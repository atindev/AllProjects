-- =============================================
-- Author:      VIVEK KUMSAR SAH
-- Create Date: 2021-06-24 09:36:42.850
-- Description: SAVING EMAIL RESPONSE USING LOGICAPP
-- =============================================
CREATE PROCEDURE [dbo].[SaveEmailResponse]
(
@IsEmail BIT,
@EmailSubject VARCHAR(Max),
@FromEmail VARCHAR(100),
@Message VARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON
    -- Insert statements for procedure here
    DECLARE @OfficialMail VARCHAR(100), @NotificationId uniqueidentifier,@subscriptionId uniqueidentifier,@FormattedMessage VARCHAR(MAX)

	IF Not EXISTS (SELECT 1 FROM Subscription (NOLOCK) WHERE PersonalEmail=@FromEmail AND IsActive=1)
		BEGIN
			SET @FormattedMessage=(SELECT REPLACE(REPLACE((SELECT LEFT(@Message,
							CASE WHEN  CHARINDEX('From:', @Message ) = 0 THEN LEN(@Message)
							ELSE CHARINDEX('From:', @Message) -1 END))
							 , CHAR(13), ''), CHAR(10), '<br/>')
							)
		END
	ELSE 
		BEGIN
			SET @FormattedMessage=(SELECT REPLACE(REPLACE((SELECT LEFT(@Message,
							CASE WHEN  CHARINDEX('West Alert System', @Message ) = 0 THEN LEN(@Message)
							ELSE CHARINDEX('West Alert System', @Message) -1 END))
							 , CHAR(13), ''), CHAR(10), '<br/>')
							)
		END

	SET @OfficialMail= (SELECT OfficialEmail FROM Subscription (NOLOCK) WHERE (PersonalEmail=@FromEmail OR OfficialEmail=@FromEmail) AND IsActive=1)
	SET @subscriptionId=(SELECT Id from Subscription (NOLOCK) WHERE (PersonalEmail=@FromEmail OR OfficialEmail=@FromEmail) AND IsActive=1)

	---Getting NotificationId
		--Getting Notification by group
		DROP TABLE IF EXISTS #NotificationByGroup
		SELECT ng.NotificationId,ng.CreatedDate INTO #NotificationByGroup FROM SubscriptionGroup sg (NOLOCK)
		INNER JOIN [dbo].[Group] g (NOLOCK) ON sg.GroupId= g.Id
		INNER JOIN NotificationGroup ng (NOLOCK) ON  ng.GroupId=g.Id
		INNER JOIN [dbo].[Notification] n (NOLOCK) ON n.Id=ng.NotificationId
		WHERE sg.SubscriptionId=@subscriptionId and n.IsEmail=1
		ORDER BY  ng.CreatedDate DESC

		--Getting Notification by Subscriber
		DROP TABLE IF EXISTS #NotificationBySubscriber
		SELECT NotificationId,CreatedDate INTO #NotificationBySubscriber FROM NotificationSubscription (NOLOCK) WHERE SubscriptionId=@subscriptionId order by CreatedDate desc

		---Getting latest NotificationId from Subscriber and SubscriberGroup
		DROP TABLE IF EXISTS #NotificationAfterfilter
		SELECT * INTO #NotificationAfterfilter FROM #NotificationByGroup
		UNION All
		SELECT * FROM #NotificationBySubscriber

	  SET @NotificationId =( SELECT TOP 1 NotificationId FROM #NotificationAfterfilter ORDER BY CreatedDate DESC )

	INSERT INTO IncomingMessage(
	Id,
	NotificationId,
	SubscriberEmail,
	IsText,
	IsVoice,
	IsWhatsApp,
	IsEmail,
	CreatedDate,
	CreatedBy,
	IsActive,
	Message,
	EmailSubject,
	FromEmail
	) 
	  VALUES( 
	   NEWID(),
	   @NotificationId,
	   @OfficialMail,
	   0,
	   0,
	   0,
	   @IsEmail,
	   GETDATE(),
	   'SP_SaveEmailResponse',
	   1,
       @FormattedMessage,
	   @EmailSubject,
	   @FromEmail
	   )
END