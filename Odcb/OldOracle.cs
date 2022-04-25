using System.Data;
using System.Data.OracleClient;

namespace Odcb
{
    class OldOracle
    {
        public DataTable GetData()
        {
            string strcon = @"Data Source=DEESMES03/MESHO03;Persist Security Info=True;User ID=wltuser;Password=wlt;";
            //@"Data Source=DEESMES03/MESHO03;Persist Security Info=True;User ID=wltuser;Password=wlt;";
            //strcon = @"SERVER=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MESST03)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=DEESMES03)));uid=wltuser;pwd=wlt;";
            using (OracleConnection con = new OracleConnection(strcon))
            {
                con.Open();
                string q = "select * from ARBEITSPLATZ";
                //"SELECT (SELECT E.WERTEXTERN_NLS1 FROM ENUMERATION_NLS E WHERE E.KONSTTYP = 'Standort' AND   E.KONSTNAME = 'Werk1') AS MDWC_PLANT, NVL(WC.ARBEITSPLATZNR, '-')   AS MDWC_ID, NVL(WC.ARBEITSPLATZBEZ,'-')  AS MDWC_NAME, NVL(RTMWC.EQUIPMENTDESC, '-') AS MDWC_RTMNAME, NVL(RTMWC.TEXT1, '-') AS MDWC_DEPARTMENT, NVL(RTMWC.TEXT2, '-') AS MDWC_INFO2, NVL(RTMWC.TEXT3, '-') AS MDWC_INFO3, NVL(RTMWC.TEXT4, '-') AS MDWC_INFO4, NVL(RTMWC.TEXT5, '-') AS MDWC_INFO5, '600' AS MDWC_LOCATION, TO_CHAR(systimestamp,'YYYY-MM-DD HH24:MI:SS.FF') AS CREATED_DATE FROM   ARBEITSPLATZ WC, RTMEQMMASTER_PROJ RTMWC WHERE  WC.ISTPEGRUPPE  = 22 AND WC.STATUSKEY = 52 AND RTMWC.STATUS = WC.STATUSKEY AND RTMWC.EQUIPMENT = WC.ARBEITSPLATZNR;";
                using (OracleCommand cm = new OracleCommand(q, con))
                {
                    using (OracleDataReader rr = cm.ExecuteReader())
                    {
                        if (rr.Read())
                        {
                            DataTable dt = CreateDataTable(rr, "CHECK");
                            return dt;
                        }
                    }
                }
            }
            return null;
        }

        private DataTable CreateDataTable(OracleDataReader rr, string tableName)
        {
            DataTable dt = new DataTable(tableName);
            for (int i = 0; i < rr.FieldCount; i++)
            {
                dt.Columns.Add(rr.GetName(i));
                //dt.Columns.Add(rr.GetName(i), rr.GetValue(i).GetType());
            }

            do
            {
                DataRow dr = dt.NewRow();

                for (int i = 0; i < rr.FieldCount; i++)
                    dr[i] = rr[i];

                dt.Rows.Add(dr);
            } while (rr.Read());

            dt.AcceptChanges();

            return dt;
        }
    }
}
