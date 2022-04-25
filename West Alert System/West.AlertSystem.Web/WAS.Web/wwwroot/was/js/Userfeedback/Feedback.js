
var imgData;
$(function () {

    $("#btn-edit-image").on('click', function () {
        var base64 = jQuery("#Base64String").val();

        var $ = function (id) { return document.getElementById(id) };

        var canvas = this.__canvas = new fabric.Canvas('canvas-drawing', {
            isDrawingMode: true
        });

        fabric.Image.fromURL(imgData, function (img) {
            img.set({
                width: parseInt(canvas.getWidth() * 30 / 100 + canvas.getWidth()),
                height: parseInt(canvas.getHeight() * 30 / 100 + canvas.getHeight()),                
                originX: 'left',
                //scaleX: canvas.getWidth() / img.width, //new update
                //scaleY: canvas.getHeight() / img.height, //new update
                scaleX: 0.8,
                scaleY: 0.8,
                originY: 'top'
            });
            canvas.setBackgroundImage(img, canvas.renderAll.bind(canvas));
        });

        fabric.Object.prototype.transparentCorners = false;

        var drawingModeEl = $('drawing-mode'),
            drawingOptionsEl = $('drawing-mode-options'),
            drawingColorEl = $('drawing-color'),
            drawingShadowColorEl = $('drawing-shadow-color'),
            drawingLineWidthEl = $('drawing-line-width'),
            drawingShadowWidth = $('drawing-shadow-width'),
            drawingShadowOffset = $('drawing-shadow-offset'),
            clearEl = $('clear-canvas');

        if (fabric.PatternBrush) {
            var vLinePatternBrush = new fabric.PatternBrush(canvas);
            vLinePatternBrush.getPatternSrc = function () {

                var patternCanvas = fabric.document.createElement('canvas');
                patternCanvas.width = patternCanvas.height = 10;
                var ctx = patternCanvas.getContext('2d');

                ctx.strokeStyle = this.color;
                ctx.lineWidth = 5;
                ctx.beginPath();
                ctx.moveTo(0, 5);
                ctx.lineTo(10, 5);
                ctx.closePath();
                ctx.stroke();

                return patternCanvas;
            };

            var hLinePatternBrush = new fabric.PatternBrush(canvas);
            hLinePatternBrush.getPatternSrc = function () {

                var patternCanvas = fabric.document.createElement('canvas');
                patternCanvas.width = patternCanvas.height = 10;
                var ctx = patternCanvas.getContext('2d');

                ctx.strokeStyle = this.color;
                ctx.lineWidth = 5;
                ctx.beginPath();
                ctx.moveTo(5, 0);
                ctx.lineTo(5, 10);
                ctx.closePath();
                ctx.stroke();

                return patternCanvas;
            };

            var squarePatternBrush = new fabric.PatternBrush(canvas);
            squarePatternBrush.getPatternSrc = function () {

                var squareWidth = 10, squareDistance = 2;

                var patternCanvas = fabric.document.createElement('canvas');
                patternCanvas.width = patternCanvas.height = squareWidth + squareDistance;
                var ctx = patternCanvas.getContext('2d');

                ctx.fillStyle = this.color;
                ctx.fillRect(0, 0, squareWidth, squareWidth);

                return patternCanvas;
            };

            var diamondPatternBrush = new fabric.PatternBrush(canvas);
            diamondPatternBrush.getPatternSrc = function () {

                var squareWidth = 10, squareDistance = 5;
                var patternCanvas = fabric.document.createElement('canvas');
                var rect = new fabric.Rect({
                    width: squareWidth,
                    height: squareWidth,
                    angle: 45,
                    fill: this.color
                });

                var canvasWidth = rect.getBoundingRect().width;

                patternCanvas.width = patternCanvas.height = canvasWidth + squareDistance;
                rect.set({ left: canvasWidth / 2, top: canvasWidth / 2 });

                var ctx = patternCanvas.getContext('2d');
                rect.render(ctx);

                return patternCanvas;
            };
        }

        if (drawingColorEl != null) {
            drawingColorEl.onchange = function () {
                var brush = canvas.freeDrawingBrush;
                brush.color = this.value;
                if (brush.getPatternSrc) {
                    brush.source = brush.getPatternSrc.call(brush);
                }
            };
        }
        if (drawingColorEl != null) {
            drawingShadowColorEl.onchange = function () {
                canvas.freeDrawingBrush.shadow.color = this.value;
            };
        }
        if (drawingColorEl != null) {
            drawingLineWidthEl.onchange = function () {
                canvas.freeDrawingBrush.width = parseInt(this.value, 10) || 1;
                this.previousSibling.innerHTML = this.value;
            };
        }
        if (drawingColorEl != null) {
            drawingShadowWidth.onchange = function () {
                canvas.freeDrawingBrush.shadow.blur = parseInt(this.value, 10) || 0;
                this.previousSibling.innerHTML = this.value;
            };
        }
        if (drawingColorEl != null) {
            drawingShadowOffset.onchange = function () {
                canvas.freeDrawingBrush.shadow.offsetX = parseInt(this.value, 10) || 0;
                canvas.freeDrawingBrush.shadow.offsetY = parseInt(this.value, 10) || 0;
                this.previousSibling.innerHTML = this.value;
            };
        }

        if (canvas.freeDrawingBrush) {
            canvas.freeDrawingBrush.color = drawingColorEl.value;
            canvas.freeDrawingBrush.source = canvas.freeDrawingBrush.getPatternSrc.call(this);
            canvas.freeDrawingBrush.width = parseInt(drawingLineWidthEl.value, 10) || 1;
            canvas.freeDrawingBrush.shadow = new fabric.Shadow({
                blur: parseInt(drawingShadowWidth.value, 10) || 0,
                offsetX: 0,
                offsetY: 0,
                affectStroke: true,
                color: drawingShadowColorEl.value,
            });
        }
    });
  
   
    var element = $("#content-pages"); // global variable

    $("#previewScreenShot").on('click', function () {

        if ($(".modal-content:visible").length > 0) {
            element = $(".modal-content:visible");
        }
        else
            element = $("#content-pages");

        

    html2canvas(element, {
        onrendered: function (canvas) {
            imgData = canvas.toDataURL("image/png");
            $("#imgscreenshot").attr("src", imgData);

            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight;

            getCanvas = canvas;
            var canvasdrawing = document.getElementById("canvas-drawing").getContext('2d');

            var destinationImage = new Image;
            destinationImage.onload = function () {
                canvasdrawing.drawImage(destinationImage, 0, 0, 600, 400);
            };
            destinationImage.src = imgData;
            $("#Base64String").val(imgData);
        }
    });
         $('.canvas-container').remove();
         
});

$("#submitFeedback").click(function () {
    if ($("#Title").val() == "" && $("#Description").val() == "") {
        return true;
    }

    var feedBackModel = {
        Username: $("#Username").val(),
        UserEmailID: $("#UserEmailID").val(),
        Title: $("#Title").val(),
        Description: $("#Description").val(),
        Base64String: $("#Base64String").val(),
        IsScreenshotRequired: $("#isScreenshotRequired").prop("checked")
    }

    showLoader();
    $.ajax({
        url: '/Feedback/SubmitFeedback',
        type: 'POST',
        datatype: "json",
        data: feedBackModel,
        success: function (result) {
            hideLoader();
            if (result.statusCode == 200) {

                $.notify({
                    // options
                    message: result.message
                }, {
                    // settings
                    type: 'success',
                        z_index: 9999,
                        placement: {
                            from: "top",
                            align: "right"
                        }
                });
            }
            else {
                $.notify({
                    // options
                    message: result.message
                }, {
                    // settings
                    type: 'error',
                        z_index: 9999,
                        placement: {
                            from: "top",
                            align: "right"
                        }
                });
            }
            
            return false;
        },
        error: function (err) {
            hideLoader();
        }
    });

    $('#feedback-modal').modal('toggle');
    return false;
});
});