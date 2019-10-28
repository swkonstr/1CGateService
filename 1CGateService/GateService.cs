using _1CGateService.WebServ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _1CGateService
{
    public class GateService
    {
        static myConn myconn;
        static WebServ.aExchange ws1C;
        static WmsReports.aReports wmsRep;
        static int idQueue = 0;
        /*
        static void Main(string[] args)
        {
            try
            {
                //Создаем соединения к главной БД и 1С сервису
                idQueue = 0;
                myconn = new myConn();
                myconn.conn.Open();
                ws1C = new aExchange();
                ws1C.Credentials = new System.Net.NetworkCredential("web", "web");
                wmsRep = new WmsReports.aReports(); //для GetStockBallance()
                wmsRep.Credentials = new System.Net.NetworkCredential("web", "web");

                DateTime dt = DateTime.Now;
                //каждые 10 секунд проверяем очередь задач в таблице _1CQueue
                //каждые 2 минуты выгружаем остатки по складу
                while (true)
                {
                    QueueProcess();
                    /*if ((DateTime.Now - dt).Minutes >= 2)
                    {
                        dt = DateTime.Now;
                        GetStockBalance();
                    }
                    GC.Collect();
                    System.Threading.Thread.Sleep(10000);
                }
            }
            catch (System.Exception ex)
            {
                //пытаемся писать ошибку в консоль, таблицу _1Clog и завершаем программу с кодом 0
                LogAdd("Ошибка!" + ex.Message);
                Console.WriteLine(ex.Message);
                myconn.conn.Close();
                ws1C.Dispose();
                System.Environment.Exit(0);
            }
        }*/



        public static void QueueProcess(Object o)
        {
            idQueue = 0;
            int type;
            int param;
            try
            {
                idQueue = 0;
                myconn = new myConn();
                myconn.conn.Open();
                ws1C = new aExchange();
                ws1C.Credentials = new System.Net.NetworkCredential("web", "web");

                OdbcCommand cmd = new OdbcCommand("SELECT TOP 100 * FROM _1CQueue WHERE status = 0 ORDER BY status , type DESC ", myconn.conn);
                OdbcDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    DateTime dtstart = DateTime.Now;
                    idQueue = 0;
                    type = (int)rd["type"];
                    idQueue = (int)rd["idQueue"];
                    OdbcCommand rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 2 WHERE idQueue =  " + idQueue.ToString(), myconn.conn);
                    rdUpd.ExecuteNonQuery();
                    if (type == 1)
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка товара или группы товаров с plid: " + param.ToString());
                        ExportNomenaclature(param); //Выгрузка в 1С товара или группы товаров
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 1, DateExec = getDate() WHERE idQueue = " + rd["idQueue"].ToString(), myconn.conn);
                        rdUpd.ExecuteNonQuery();
                        LogAdd("Выгрузка товара или группы товаров с plid: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                    if (type == 2 || type == 7)
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка поставки с wbMid: " + param.ToString());
                        ExportInvoice(param); //Выгрузка в 1С инвойса
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 1, DateExec = getDate() WHERE idQueue = " + rd["idQueue"].ToString(), myconn.conn);
                        rdUpd.ExecuteNonQuery();
                        LogAdd("Выгрузка поставки с wbMid: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                    if (type == 3)
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка отгрузки с wbMid: " + param.ToString());
                        ExportShipping(param); //Выгрузка в 1С накладной на отгрузку
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status =1, DateExec = getDate() WHERE idQueue = " + rd["idQueue"].ToString(), myconn.conn);
                        rdUpd.ExecuteNonQuery();
                        LogAdd("Выгрузка отгрузки с wbMid: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                    if (type == 4)
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка накладной на отгрузку ПОКОМЛЕКТНО с wbMid: " + param.ToString());
                        ExportShippingDet(param); //Выгрузка в 1С накладной на отгрузку ПОКОМЛЕКТНО
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 1, DateExec = getDate() WHERE idQueue = " + rd["idQueue"].ToString(), myconn.conn);
                        rdUpd.ExecuteNonQuery();
                        LogAdd("Выгрузка накладной на отгрузку ПОКОМЛЕКТНО с wbMid: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                    if (type == 5)
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка задания на сборку/разборку с wbMid: " + param.ToString());
                        InternalOrder_New(param); //Выгрузка в 1С задания на сборку/разборку
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 1, DateExec = getDate() WHERE idQueue = " + rd["idQueue"].ToString(), myconn.conn);
                        rdUpd.ExecuteNonQuery();
                        LogAdd("Выгрузка задания на сборку/разборку с wbMid: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                    if (type == 6 && (DateTime.Now.Hour > 0 || DateTime.Now.Hour < 9))
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка маршрута с DelivRouteId: " + param.ToString());
                        Route(param); //Выгрузка в 1С маршрутов
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 1, DateExec = getDate() WHERE idQueue = " + idQueue.ToString(), myconn.conn);
                        rdUpd.ExecuteScalar();
                        LogAdd("Выгрузка маршрута с DelivRouteId: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                    if (type == 8)
                    {
                        dtstart = DateTime.Now;
                        param = (int)rd["nParam1"];
                        LogAdd("Выгрузка накладной списания с wbMid: " + param.ToString());
                        ExportCancellation(param); //Выгрузка накладной списания
                        rdUpd = new OdbcCommand("UPDATE _1CQueue SET status =1, DateExec = getDate() WHERE idQueue =  " + rd["idQueue"].ToString(), myconn.conn);
                        rdUpd.ExecuteNonQuery();
                        LogAdd("Выгрузка накладной списания с wbMid: " + param.ToString() + " завершена успешно - " + System.DateTime.Now.Subtract(dtstart));
                        rdUpd = null;
                    }
                }
                rd.Close();
                rd = null;
            }
            catch (System.Exception ex)
            {
                //Если ошибка возникла в функции экспорта списания/поставки/отгрузки, ставим статус 2 для дальнейшего разбора в ручном режиме.
                if (idQueue > 0 && ex.Message.IndexOf("Не удалось передать накладную") >= 0)
                {
                    OdbcCommand rdUpd = new OdbcCommand("UPDATE _1CQueue SET status = 2 WHERE idQueue =  " + idQueue.ToString(), myconn.conn);
                    rdUpd.ExecuteNonQuery();
                    rdUpd = null;
                    LogAdd(ex.Message);
                }
                else
                {
                    LogAdd("Ошибка в процедуре QueueProcess()!  " + ex.Message);
                }
            }
            GC.Collect();
        }

        public static void StockBalanceProcess(Object o)
        {
            idQueue = 0;
            myConn myconnW = new myConn();
            myconnW.conn.Open();
            wmsRep = new WmsReports.aReports();
            wmsRep.Credentials = new System.Net.NetworkCredential("web", "web");

            DateTime dtstart = DateTime.Now;
            WmsReports.StockBalanceLine[] sbalance;

            sbalance = wmsRep.GetStockBalance(DateTime.Now, "knit_ws", "");
            string logstr = "Warehouse. Получение остатков из WMS - " + System.DateTime.Now.Subtract(dtstart);
            OdbcCommand cmdUpdW = new OdbcCommand("Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr + "')", myconnW.conn);
            cmdUpdW.ExecuteNonQuery();

            if (sbalance.Length > 0)
            {
                dtstart = DateTime.Now;
                //cmdUpdW.CommandText = "UPDATE _1CNomenclature SET StockBalanceTemp = 0; commit;";
                cmdUpdW = new OdbcCommand ("UPDATE _1CNomenclature SET StockBalanceTemp = 0; commit;",myconnW.conn);
                cmdUpdW.ExecuteNonQuery();
                string logstr2 = "Warehouse. Обнуление StockBalanceTemp - " + System.DateTime.Now.Subtract(dtstart);
                //cmdUpdW.CommandText = "Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr2 + "')";
                cmdUpdW = new OdbcCommand ("Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr2 + "')", myconnW.conn);
                cmdUpdW.ExecuteNonQuery();

                dtstart = DateTime.Now;
                //cmdUpdW.CommandText = "TRUNCATE TABLE Wms_Good_Status; COMMIT;";
                cmdUpdW = new OdbcCommand("TRUNCATE TABLE Wms_Good_Status; COMMIT;", myconnW.conn);
                cmdUpdW.ExecuteNonQuery();
                logstr2 = "Warehouse. Очистка Wms_Good_Status - " + System.DateTime.Now.Subtract(dtstart);
                //cmdUpdW.CommandText = "Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr2 + "')";
                cmdUpdW = new OdbcCommand("Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr2 + "')", myconnW.conn);
                cmdUpdW.ExecuteNonQuery();

                dtstart = DateTime.Now;
                foreach (WmsReports.StockBalanceLine sb in sbalance)
                {
                    string uid = sb.NomenclatureID;
                    int ynSet;
                    if (sb.Set) ynSet = 1; else ynSet = 0;
                    decimal qnt = sb.Quantity;
                    cmdUpdW = new OdbcCommand("Update _1CNomenclature SET YnSet =  " + ynSet.ToString() + " ,  DateUpd = Now() , StockBalanceTemp = StockBalanceTemp + " + qnt.ToString().Replace(',', '.') + " where Guid_nom = '" + uid + "';commit;", myconnW.conn);
                    cmdUpdW.ExecuteNonQuery();
                    cmdUpdW = new OdbcCommand("Insert into Wms_Good_Status (statusGuid , balance , Guid_nom) values ('" + sb.StatusItemID.ToString() + "'," + qnt.ToString().Replace(',', '.') + ",'" + uid + "');commit;", myconnW.conn);
                    cmdUpdW.ExecuteNonQuery();
                    cmdUpdW = null;
                }

                logstr = "Warehouse. Обработка массива из " + sbalance.Length + " товаров. - " + System.DateTime.Now.Subtract(dtstart);
                cmdUpdW = new OdbcCommand("Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr + "')", myconnW.conn);
                cmdUpdW.ExecuteNonQuery();

                dtstart = DateTime.Now;
                cmdUpdW = new OdbcCommand("UPDATE _1CNomenclature  SET DateUpd = Now(), StockBalance = StockBalanceTemp;commit;", myconnW.conn);
                cmdUpdW.ExecuteNonQuery();
                logstr = "Warehouse. Обновление дат в _1CNomenclature - " + System.DateTime.Now.Subtract(dtstart);
                cmdUpdW = new OdbcCommand("Insert into _1Clog (toIdQueue,MsgLog) values (0,'" + logstr + "')", myconnW.conn);
                cmdUpdW.ExecuteNonQuery();
                cmdUpdW = null;

            }
            GC.Collect();

        }

        #region Основные функции QueueProcess().
        static void ExportNomenaclature(int plid) //выгрузка в 1С товара или группы товаров 
        {
            LogAdd("ExportNomenaclature(plid:" + plid.ToString() + ")");

            OdbcCommand cmd = new OdbcCommand("Select * from priceList where plid = " + plid.ToString(), myconn.conn);
            OdbcDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                if ((int)rd["yngroup"] == 0)
                {
                    ExportNomenclatureSimple(plid);
                }
                else
                {
                    ExportGroupNomenclature(plid);
                }
            }
            rd.Close();
            cmd.Dispose();
        }


        static void ExportInvoice(int wbmid, bool isSborka = false) //выгрузка в 1С инвойса 
        {
            LogAdd("ExportInvoice(wbmid:" + wbmid.ToString() + ")");

            OdbcCommand cmd = new OdbcCommand("Select wbmain.*,  isnull(faktknitindate, current date) as faktDate, isnull(wbmain.planknitindate, current date) as planDate, wbGuid, isnull(_1cwbmain.towbmid,0) as _1ctowbmid from wbmain  left join _1cwbmain on wbmid = towbmid where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();

            Guid wbGuid;
            WebServ.axArrivalPlan plan = new axArrivalPlan();
            WebServ.axGoodsLine gl = null;
            WebServ.axGoodsLine[] glArr = new axGoodsLine[0];

            if (rdMain.Read())
            {
                if ((int)rdMain["_1ctowbmid"] == 0)
                    wbGuid = Guid.NewGuid();
                else
                    wbGuid = (Guid)rdMain["wbGuid"];
                plan.ID = wbGuid.ToString();
                plan.Date = (DateTime)rdMain["chDate"];
                plan.ExpectedDate = (DateTime)rdMain["faktDate"];
                plan.CounterpartyID = GetSupplierID((int)rdMain["tosupplierid"]).ToString();
                if (rdMain["toFirmOfficeId"].ToString().Length > 0)
                    plan.OrganizationID = GetOrganizationID((int)rdMain["toFirmOfficeId"]);
                else
                    plan.OrganizationID = "1b5e620d-d38d-11e4-b166-c860006e2886";
                if (wbmid.ToString().Length == 6)
                    plan.Number = wbmid.ToString().Substring(0, 2) + "." + wbmid.ToString().Substring(2, 4);
                else
                    plan.Number = wbmid.ToString().Substring(0, 3) + "." + wbmid.ToString().Substring(3, 4);
                plan.OrderSource = "";
                if (isSborka)
                    plan.OrderSource = "ПриемкаИзПроизводства";
                OdbcCommand cmdDet = new OdbcCommand("Select  (Select towbmid from wbdet wout where wout.wbdetid= wc.wbOutDet ) as wbOut, isnull(wc.cnt,0) as cnt, wbdet.* , isnull(_1c.toplid,0) as _1cToplid , Guid_nom   from wbdet left join _1CNomenclature _1c on wbdet.toplid = _1c.toplid left join WbCrossDocking wc on wc.wbDelivDet=wbdet.wbdetid  where towbmid = " + wbmid.ToString() + " order by wbOut desc", myconn.conn);
                OdbcDataReader rdDet = cmdDet.ExecuteReader();
                int cntRow = 0;
                while (rdDet.Read())
                {
                    cntRow++;
                    Guid nomenGuid;
                    int toplid = (int)rdDet["Toplid"];
                    nomenGuid = ExportNomenclatureSimple(toplid, true);
                    //                    if ((int)rdDet["_1cToplid"] == 0)
                    //                        nomenGuid = ExportNomenclatureSimple(toplid,true);
                    //                    else
                    //                        nomenGuid = (Guid)rdDet["Guid_nom"];
                    gl = null;
                    gl = new axGoodsLine();
                    gl.NomenclatureID = nomenGuid.ToString();
                    gl.Quantity = (decimal)rdDet["numberdistr"];
                    gl.QuantityUnit = (decimal)rdDet["numberdistr"];
                    //gl.Quantity = 55;
                    //gl.QuantityUnit = 55;
                    gl.StorangeUnitID = GetMeansBaseGuid((int)rdDet["toplid"]).ToString();
                    gl.LineNum = Convert.ToDecimal(rdDet["wbdetid"]);
                    gl.SpecificationID = "";
                    gl.Set = false;
                    gl.Amount = (decimal)rdDet["prUnit"];
                    string wbOut = rdDet["wbout"].ToString();
                    if (wbOut.Length > 0)
                    {
                        if (cntRow == 1) ExportShipping(Convert.ToInt32(wbOut));
                        decimal cntCross = (decimal)rdDet["cnt"];
                        if (cntCross > 0 && gl.Quantity > cntCross)
                        {
                            axGoodsLine glDbl = new axGoodsLine();
                            glDbl.LineNum = gl.LineNum;
                            glDbl.NomenclatureID = gl.NomenclatureID;
                            glDbl.QualityID = gl.QualityID;
                            glDbl.Set = gl.Set;
                            glDbl.SpecificationID = gl.SpecificationID;
                            glDbl.StorangeUnitID = gl.StorangeUnitID;
                            glDbl.Amount = gl.Amount;
                            decimal qnty = gl.Quantity;

                            gl.aShippingPlanID = GetWbGuid(wbOut);
                            gl.Quantity = cntCross;
                            gl.QuantityUnit = cntCross;
                            glDbl.Quantity = qnty - cntCross;
                            glDbl.QuantityUnit = glDbl.Quantity;
                            Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                            glArr[cntRow - 1] = glDbl;
                            cntRow++;
                        }
                        if (gl.Quantity == cntCross)
                        {
                            gl.aShippingPlanID = GetWbGuid(wbOut);
                        }
                    }
                    //gl.QualityID = "48148bd4-15ec-4245-ae04-0ee6285851d1";
                    //gl.aShippingPlanID = "27e27f54-1d85-40b1-9727-cfd9a5d2085a";
                    Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                    glArr[cntRow - 1] = gl;
                }
                rdDet.Close();
                rdDet = null;
                plan.GoodsLine = glArr;
                plan.CargoSpace = false;
                bool res = ws1C.SetArrivalPlan(plan, "knit_ws");
                if (res)
                {
                    OdbcCommand cmdSave;
                    if ((int)rdMain["_1ctowbmid"] == 0)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + rdMain["wbmid"].ToString() + ",'" + wbGuid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new SyntaxErrorException("Не удалось передать накладную поставки c wbmid:" + wbmid.ToString());
                }
            }
            rdMain.Close();
            rdMain = null;
        }


        static string ExportShipping(int wbmid, bool isSborka = false) //выгрузка в 1С накладной на отгрузку 
        {
            LogAdd("ExportShipping(wbmid:" + wbmid.ToString() + ")");
            string retGuid = "";
            Guid wbGuid;
            OdbcCommand cmd = new OdbcCommand("Select  (select WmsGuid from delivroutes where DelivRouteID = (Select toDelivRouteID from WbMainDeliv where toWbMid = wbmid)) as RouteGuid ,  wbmain.*  , wbGuid, isnull(_1cwbmain.towbmid,0) as _1ctowbmid from wbmain  left join _1cwbmain on wbmid = towbmid where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();
            WebServ.axShippingPlan plan = new axShippingPlan();
            WebServ.axGoodsLine gl = null;
            WebServ.axGoodsLine[] glArr = new axGoodsLine[0];
            if (rdMain.Read())
            {
                if ((int)rdMain["_1ctowbmid"] == 0) wbGuid = Guid.NewGuid(); else wbGuid = (Guid)rdMain["wbGuid"];
                retGuid = wbGuid.ToString();
                plan.ID = wbGuid.ToString();
                plan.Date = (DateTime)rdMain["DateControl"];
                plan.ExpectedDate = (DateTime)rdMain["DateControl"];
                plan.CounterpartyID = plan.CounterpartyID = GetSupplierID((int)rdMain["tosupplierid"]).ToString();
                if (rdMain["toFirmOfficeId"].ToString().Length > 0)
                    plan.OrganizationID = GetOrganizationID((int)rdMain["toFirmOfficeId"]);
                else
                    plan.OrganizationID = "1b5e620d-d38d-11e4-b166-c860006e2886";
                if (wbmid.ToString().Length == 6)
                    plan.Number = wbmid.ToString().Substring(0, 2) + "." + wbmid.ToString().Substring(2, 4);
                else
                    plan.Number = wbmid.ToString().Substring(0, 3) + "." + wbmid.ToString().Substring(3, 4);
                plan.RouteID = rdMain["RouteGuid"].ToString();
                if (isSborka)
                    plan.ShipmentDirection = "ОтгрузкаВПроизводство";
                OdbcCommand cmdDet = new OdbcCommand("Select wbdet.* , GoodFullName(wbdet.Toplid) as FullName ,  isnull(_1c.toplid,0) as _1cToplid , Guid_nom  from wbdet left join _1CNomenclature _1c on wbdet.toplid = _1c.toplid  where towbmid = " + wbmid.ToString(), myconn.conn);
                OdbcDataReader rdDet = cmdDet.ExecuteReader();
                int cntRow = 0;
                while (rdDet.Read())
                {
                    Guid nomenGuid;
                    int toplid = (int)rdDet["Toplid"];
                    Console.WriteLine("   " + rdDet["FullName"].ToString());
                    if (isCanExport(toplid))
                    {
                        cntRow++;
                        if ((int)rdDet["_1cToplid"] == 0)
                            nomenGuid = ExportNomenclatureSimple(toplid, true);
                        else
                            nomenGuid = (Guid)rdDet["Guid_nom"];
                        gl = null;
                        gl = new axGoodsLine();
                        //gl.aShippingPlanID = "";
                        gl.NomenclatureID = nomenGuid.ToString();
                        gl.Quantity = (decimal)rdDet["numberdistr"];
                        gl.QuantityUnit = (decimal)rdDet["numberdistr"];
                        //gl.Quantity = 22;
                        //gl.QuantityUnit = 22;
                        gl.StorangeUnitID = GetMeansBaseGuid((int)rdDet["toplid"]).ToString();
                        gl.LineNum = Convert.ToDecimal(rdDet["wbdetid"]);
                        gl.Amount = (decimal)rdDet["prUnit"];
                        gl.Set = false;
                        gl.SpecificationID = "";
                        //                        gl.QualityID = "48148bd4-15ec-4245-ae04-0ee6285851d1";//кондиция

                        Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                        glArr[cntRow - 1] = gl;
                    }
                }
                rdDet.Close();
                rdDet = null;
                plan.GoodsLine = glArr;
                plan.CargoSpace = false; //транзит
                bool res = ws1C.SetShippingPlan(plan, "knit_ws");
                if (res)
                {
                    OdbcCommand cmdSave;
                    if ((int)rdMain["_1ctowbmid"] == 0)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + rdMain["wbmid"].ToString() + ",'" + wbGuid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new SyntaxErrorException("Не удалось передать накладную отгрузки  " + wbmid.ToString());
                }

            }
            rdMain.Close();
            rdMain = null;
            return retGuid;
        }


        static void ExportShippingDet(int wbmid) //выгрузка в 1С накладной на отгрузку ПОКОМЛЕКТНО 
        {
            LogAdd("ExportShippingDet(wbmid:" + wbmid.ToString() + ")");
            Guid wbGuid;
            OdbcCommand cmd = new OdbcCommand("Select wbmain.*  , wbGuid, isnull(_1cwbmain.towbmid,0) as _1ctowbmid from wbmain  left join _1cwbmain on wbmid = towbmid where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();
            WebServ.axShippingPlan plan = new axShippingPlan();
            WebServ.axGoodsLine gl = null;
            WebServ.axGoodsLine[] glArr = new axGoodsLine[0];
            if (rdMain.Read())
            {
                if ((int)rdMain["_1ctowbmid"] == 0) wbGuid = Guid.NewGuid(); else wbGuid = (Guid)rdMain["wbGuid"];
                plan.ID = wbGuid.ToString();
                plan.Date = (DateTime)rdMain["DateControl"];
                plan.CounterpartyID = plan.CounterpartyID = GetSupplierID((int)rdMain["tosupplierid"]).ToString();
                plan.Number = rdMain["WbNum"].ToString();
                OdbcCommand cmdDet = new OdbcCommand("Select wbdet.* , isnull(_1c.toplid,0) as _1cToplid , Guid_nom  from wbdet_compl as wbdet  left join _1CNomenclature _1c on wbdet.toplid = _1c.toplid  where towbmid = " + wbmid.ToString(), myconn.conn);
                OdbcDataReader rdDet = cmdDet.ExecuteReader();
                int cntRow = 0;
                while (rdDet.Read())
                {
                    cntRow++;
                    Guid nomenGuid;
                    if ((int)rdDet["_1cToplid"] == 0)
                        nomenGuid = ExportNomenclatureSimple((int)rdDet["Toplid"]);
                    else
                        nomenGuid = (Guid)rdDet["Guid_nom"];
                    gl = null;
                    gl = new axGoodsLine();
                    gl.NomenclatureID = nomenGuid.ToString();
                    gl.Quantity = (decimal)rdDet["numberdistr"];
                    gl.QuantityUnit = 2;
                    gl.StorangeUnitID = GetMeansBaseGuid((int)rdDet["toplid"]).ToString();
                    gl.LineNum = Convert.ToDecimal(rdDet["wbdetid"]);
                    gl.Amount = (decimal)rdDet["prUnit"];
                    Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                    glArr[cntRow - 1] = gl;
                }
                rdDet.Close();
                rdDet = null;
                plan.GoodsLine = glArr;
                bool res = ws1C.SetShippingPlan(plan, "knit_ws");
                if (res)
                {
                    OdbcCommand cmdSave;
                    if ((int)rdMain["_1ctowbmid"] == 0)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + rdMain["wbmid"].ToString() + ",'" + wbGuid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                    }
                }
            }
            rdMain.Close();
            rdMain = null;
        }


        static void InternalOrder_New(int wbmid) //сборка разборка 
        {
            LogAdd("InternalOrder_New(wbmid:" + wbmid.ToString() + ")");

            WebServ.axSpecification spec = new axSpecification();
            WebServ.axSpecification spec2 = new axSpecification();
            WebServ.axSpecificationComponents specComp = new axSpecificationComponents();
            WebServ.axSpecificationComponents[] SpecArr = new axSpecificationComponents[0];

            WebServ.axInternalOrder ord;
            WebServ.axInternalOrder ord2;

            Guid guidPlid = Guid.Empty;
            Guid guidPlidSpec = Guid.Empty;
            ord = new axInternalOrder();
            ord.ID = GetUidWb(wbmid).ToString();
            OdbcCommand cmd = new OdbcCommand("Select * from wbmain where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                int wbSlave = (int)rd["toLinkedWbMid"];
                ExportInvoice(wbmid, true);
                ExportShipping(wbSlave, true);
            }
            rd.Close();
            rd = null;
        }


        static void Route(int delivrouteid) //маршруты 
        {
            LogAdd("Route(delivrouteid:" + delivrouteid.ToString() + ")");
            string deliv = "";

            WebServ.axPassage pas = new axPassage();
            WebServ.axVehicles veh = new axVehicles();
            WebServ.axRoute rt = new axRoute();

            int cnt = 0;
            try
            {
                OdbcCommand cmd = new OdbcCommand("select DelivTransport.WmsGuid as transGuid, delivroutes.WmsGuid  as delivGuid, *  ,  (select first Name from Supplier where sid = delivroutes.todriverid ) as DriverName from delivroutes inner join DelivTransport on  dtid = todtid where delivrouteid = " + delivrouteid.ToString(), myconn.conn);
                OdbcDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    string guid = rd["transGuid"].ToString();
                    if (guid.Length == 0)
                    {
                        guid = Guid.NewGuid().ToString();
                        OdbcCommand cmdUpd = new OdbcCommand("Update DelivTransport  SET WmsGuid = '" + guid + "' where dtid=" + rd["dtid"].ToString(), myconn.conn);
                        cmdUpd.ExecuteNonQuery();
                    }
                    if (cnt == 0)
                    {
                        veh.ID = guid;
                        veh.Model = rd["model"].ToString();
                        veh.Name = rd["model"].ToString();
                        veh.StateNumber = rd["number"].ToString();
                        veh.LoadCapacity = 1;
                        veh.Volume = 0;
                        veh.NumberSeats = 1;
                        bool res1 = ws1C.SetVehicles(veh, "knit_ws");
                        Console.WriteLine(res1);
                        cnt++;
                    }

                    pas.Date = (DateTime)rd["PlanDate"];
                    pas.Driver = rd["DriverName"].ToString();
                    pas.DriverDocument = "";
                    pas.Number = rd["delivrouteid"].ToString();
                    pas.VehiclesID = guid;
                    pas.IsMarked = false;

                    deliv = rd["delivGuid"].ToString();
                    if (deliv.Length == 0)
                    {
                        deliv = Guid.NewGuid().ToString();
                        OdbcCommand cmdUpd = new OdbcCommand("Update delivroutes  SET WmsGuid = '" + deliv + "' where DelivRouteID=" + rd["DelivRouteID"].ToString(), myconn.conn);
                        cmdUpd.ExecuteNonQuery();
                    }
                    pas.ID = deliv;
                }
                rd.Close();
                rd = null;
                if (deliv.Length == 0) return;
                cmd = null;
                cmd = new OdbcCommand("Select  toWbMid  from WbMainDeliv where toDelivRouteID= " + delivrouteid.ToString(), myconn.conn);
                OdbcDataReader rdWb = cmd.ExecuteReader();
                WebServ.axOrder[] ord = new axOrder[0];
                int cntOrd = 0;
                while (rdWb.Read())
                {
                    //                    ExportShipping((int)rd["towbmid"]);
                    axOrder o = new axOrder();
                    cntOrd++;
                    Array.Resize<axOrder>(ref ord, cntOrd);
                    o.OrderID = GetWbGuid(rdWb["towbmid"].ToString());
                    ord[cntOrd - 1] = o;
                }
                pas.Order = ord;
                rdWb.Close();
                bool res = ws1C.SetPassage(pas, "knit_ws");
                Console.WriteLine(res);
                cmd = null;
                rdWb = null;
            }
            catch (System.Exception ex)
            {
                LogAdd(System.DateTime.Now.ToString() + " > Ошибка в процедуре Route.  " + ex.Message);
                Console.WriteLine("Ошибка в процедуре Route");
                Console.WriteLine(ex.Message);
            }
        }


        static void ExportCancellation(int wbmid) //Списание товара 
        {
            LogAdd("ExportCancellation(wbmid:" + wbmid.ToString() + ")");
            Guid wbGuid;
            OdbcCommand cmd = new OdbcCommand("Select wbmain.* , isnull(wbmain.planknitindate, current date) as planDate   , wbGuid, isnull(_1cwbmain.towbmid,0) as _1ctowbmid from wbmain  left join _1cwbmain on wbmid = towbmid where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();
            WebServ.axArrivalPlan plan = new axArrivalPlan();
            WebServ.axGoodsLine gl = null;
            WebServ.axGoodsLine[] glArr = new axGoodsLine[0];
            if (rdMain.Read())
            {
                if ((int)rdMain["_1ctowbmid"] == 0) wbGuid = Guid.NewGuid(); else wbGuid = (Guid)rdMain["wbGuid"];
                plan.ID = wbGuid.ToString();
                plan.Date = (DateTime)rdMain["chDate"];
                plan.ExpectedDate = (DateTime)rdMain["planDate"];
                plan.CounterpartyID = GetSupplierIDCancellation((int)rdMain["tosupplierid"]).ToString();
                plan.Number = rdMain["WbNum"].ToString();
                OdbcCommand cmdDet = new OdbcCommand("SELECT wbdet.* , isnull(_1c.toplid,0) AS _1cToplid , Guid_nom FROM wbdet LEFT JOIN _1CNomenclature _1c on wbdet.toplid = _1c.toplid  where towbmid = " + wbmid.ToString(), myconn.conn);
                OdbcDataReader rdDet = cmdDet.ExecuteReader();
                int cntRow = 0;
                while (rdDet.Read())
                {
                    cntRow++;
                    Guid nomenGuid;
                    int toplid = (int)rdDet["Toplid"];
                    if ((int)rdDet["_1cToplid"] == 0)
                        nomenGuid = ExportNomenclatureSimpleCancellation(toplid, true);
                    else
                        nomenGuid = (Guid)rdDet["Guid_nom"];
                    gl = null;
                    gl = new axGoodsLine();
                    gl.NomenclatureID = nomenGuid.ToString();
                    gl.Quantity = (decimal)rdDet["numberdistr"];
                    gl.QuantityUnit = (decimal)rdDet["numberdistr"];
                    //gl.Quantity = 55;
                    //gl.QuantityUnit = 55;
                    gl.StorangeUnitID = GetMeansBaseGuid((int)rdDet["toplid"]).ToString();
                    gl.LineNum = Convert.ToDecimal(rdDet["wbdetid"]);
                    gl.SpecificationID = "";
                    gl.Set = false;
                    gl.Amount = (decimal)rdDet["prUnit"];
                    //gl.aShippingPlanID = "27e27f54-1d85-40b1-9727-cfd9a5d2085a";
                    Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                    glArr[cntRow - 1] = gl;
                }
                rdDet.Close();
                rdDet = null;
                plan.GoodsLine = glArr;
                plan.CargoSpace = false;
                bool res = ws1C.SetArrivalPlan(plan, "knit_ws");
                if (res)
                {
                    OdbcCommand cmdSave;
                    if ((int)rdMain["_1ctowbmid"] == 0)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + rdMain["wbmid"].ToString() + ",'" + wbGuid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new SyntaxErrorException("Не удалось передать накладную списания  " + wbmid.ToString());
                }
            }
            rdMain.Close();
            rdMain = null;
        }

        #endregion

               
        #region Вспомогательные функции.
        static Guid ExportNomenclatureSimple(int plid, bool rewrite = true, bool isSpec = false) //выгрузка в 1С одной позиции товара 
        {
            LogAdd("ExportNomenclatureSimple(plid:" + plid.ToString() + ")");
            Guid nomen_guid;
            Guid nomen_mens;
            string barcode;
            int prbarID;
            bool res;
            WebServ.axNomenclature nm = new axNomenclature();
            WebServ.axStorangeUnit unit = new axStorangeUnit();
            OdbcCommand cmd2 = new OdbcCommand("Select first * from _1CNomenclature where toPlid = " + plid.ToString(), myconn.conn);
            OdbcDataReader rdPlid = cmd2.ExecuteReader();
            if (!rdPlid.Read() || rewrite)
            {
                if (rdPlid.HasRows) nomen_guid = (Guid)rdPlid["Guid_nom"]; else nomen_guid = Guid.NewGuid();
                nm.ID = nomen_guid.ToString();
                OdbcDataReader rdPrList = new OdbcCommand("Select isnull ((select name from goodname where gnid = tognid), '') + ' ' + 	isnull( (select name from firms where tofirmid = Firmid ), '' ) +  ' ' + kind	as FullName , * from pricelist where plid = " + plid.ToString(), myconn.conn).ExecuteReader();
                if (rdPrList.Read())
                {

                    nm.Name = rdPrList["FullName"].ToString();
                    nm.FullName = rdPrList["FullName"].ToString();
                    nm.GroupID = ExportTree((int)rdPrList["parent"]).ToString();
                    nm.IsGroup = false;
                    nm.Articul = rdPrList["artikul"].ToString();
                    nm.CodeCIS = rdPrList["plid"].ToString();
                    //                    nm.SpecificationID = "";
                    //                    nm.SetSpecified = false;
                    //                    nm.Set = false;
                    if (isSpec)
                    {
                        //                        nm.SetSpecified = true;
                        //                        nm.Set = true;
                    }
                    nm.ClassifierUnitsMesurementID = "222";
                    nm.weight = 0;
                    nm.volume = 44;
                    res = ws1C.SetNomenclature(nm, "knit_ws");
                    if (!res) throw new SyntaxErrorException("Не удалось записать товар plid =  " + plid.ToString());
                    OdbcCommand cmdSave = null;
                    if (!rdPlid.HasRows)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CNomenclature (toPlid,Guid_nom) values (" + plid.ToString() + " , '" + nomen_guid.ToString() + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                        cmdSave.Dispose();
                        //                        LogAdd("Товару с кодом " + plid.ToString() + " присвоен в WMS код " + nomen_guid.ToString());
                    }
                }
                rdPrList.Close();
            }
            else
            {
                Guid result = (Guid)rdPlid["Guid_nom"];
                nomen_guid = result;
            }
            rdPlid.Close();
            rdPlid = null;
            OdbcCommand cmd = new OdbcCommand("call GetBarCode (" + plid.ToString() + ")", myconn.conn);
            OdbcDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                barcode = rd["barcode1"].ToString();
                prbarID = (int)rd["prbarID"];
                if (barcode.Length == 0) { barcode = rd["barcodeCorp"].ToString(); prbarID = (int)rd["prbarCorpID"]; }
                OdbcDataReader rdMens = new OdbcCommand("Select * from _1CBarCode where toPrbarID = " + prbarID.ToString(), myconn.conn).ExecuteReader();
                if (!rdMens.Read() || rewrite)
                {
                    if (rdMens.HasRows) nomen_mens = (Guid)rdMens["Guid_mens"]; else nomen_mens = Guid.NewGuid();
                    unit.ID = nomen_mens.ToString();
                    unit.OwnerID = nomen_guid.ToString();
                    unit.volume = (decimal)rd["PVolume"];
                    if (unit.volume == 0) unit.volume = 1;
                    unit.weight = (decimal)rd["GrossWeight"];
                    if (unit.weight == 0) unit.weight = 1;
                    unit.ClassifierID = "222";
                    unit.Name = rd["name"].ToString();
                    unit.coefficient = (decimal)rd["ScaleFactor"];
                    unit.BarCode = barcode;
                    unit.Code = prbarID.ToString();
                    unit.width = (decimal)rd["width"];
                    unit.height = (decimal)rd["height"];
                    unit.depth = (decimal)rd["depth"];
                    if (unit.volume == 1 && unit.width > 0 && unit.height > 0 && unit.depth > 0) unit.volume = (decimal)(unit.width * unit.height * unit.depth) / 1000;
                    if (rd["isNotBase"].ToString() == "0") unit.main = true; else unit.main = false;
                    res = ws1C.SetStorangeUnit(unit, "knit_ws");

                    if (!res) if (!res) throw new SyntaxErrorException("Не удалось записать штрих-код  PrbarID  " + prbarID.ToString());
                    if (!rdMens.HasRows)
                    {
                        OdbcCommand cmdSave = new OdbcCommand("Insert into _1CBarCode (toPrbarID,Guid_mens) values (" + prbarID.ToString() + " , '" + nomen_mens + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                        cmdSave.Dispose();
                    }
                }
                rdMens.Close();
            }
            rd.Close();
            rd = null;
            return nomen_guid;
        }


        static Guid ExportNomenclatureSimpleCancellation(int plid, bool rewrite = true, bool isSpec = false) //вариант функции который работал в 1CGate_N 
        {
            LogAdd("ExportNomenclatureSimpleCancellation(plid:" + plid + ")");
            Guid nomen_guid;
            Guid nomen_mens;
            string barcode;
            int prbarID;
            bool res;
            WebServ.axNomenclature nm = new axNomenclature();
            WebServ.axStorangeUnit unit = new axStorangeUnit();

            OdbcCommand cmd2 = new OdbcCommand("Select first * from _1CNomenclature where toPlid = " + plid.ToString(), myconn.conn);
            OdbcDataReader rdPlid = cmd2.ExecuteReader();
            if (!rdPlid.Read() || rewrite)
            {
                if (rdPlid.HasRows) nomen_guid = (Guid)rdPlid["Guid_nom"]; else nomen_guid = Guid.NewGuid();
                nm.ID = nomen_guid.ToString();
                OdbcDataReader rdPrList = new OdbcCommand("Select isnull ((select name from goodname where gnid = tognid), '') + ' ' + 	isnull( (select name from firms where tofirmid = Firmid ), '' ) +  ' ' + kind	as FullName , * from pricelist where plid = " + plid.ToString(), myconn.conn).ExecuteReader();
                if (rdPrList.Read())
                {

                    nm.Name = rdPrList["FullName"].ToString();
                    nm.FullName = rdPrList["FullName"].ToString();
                    nm.GroupID = ExportTree((int)rdPrList["parent"]).ToString();
                    nm.IsGroup = false;
                    nm.Articul = rdPrList["artikul"].ToString();
                    nm.CodeCIS = rdPrList["plid"].ToString();
                    nm.SpecificationID = "";
                    nm.SetSpecified = false;
                    nm.Set = false;
                    if (isSpec)
                    {
                        nm.SetSpecified = true;
                        nm.Set = true;
                    }
                    nm.ClassifierUnitsMesurementID = "31";
                    nm.weight = 0;
                    nm.volume = 44;
                    res = ws1C.SetNomenclature(nm, "knit_ws");
                    if (!res) throw new SyntaxErrorException("Не удалось записать товар plid =  " + plid.ToString());
                    OdbcCommand cmdSave = null;
                    if (!rdPlid.HasRows)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CNomenclature (toPlid,Guid_nom) values (" + plid.ToString() + " , '" + nomen_guid.ToString() + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                        cmdSave.Dispose();
                        LogAdd(System.DateTime.Now.ToString() + " > Товару с кодом " + plid.ToString() + " присвоен в WMS код " + nomen_guid.ToString());
                    }
                }

            }
            else
            {
                Guid result = (Guid)rdPlid["Guid_nom"];
                nomen_guid = result;
            }
            rdPlid.Close();
            rdPlid = null;
            OdbcCommand cmd = new OdbcCommand("call GetBarCode (" + plid.ToString() + ")", myconn.conn);
            OdbcDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                barcode = rd["barcode1"].ToString();
                prbarID = (int)rd["prbarID"];
                if (barcode.Length == 0) { barcode = rd["barcodeCorp"].ToString(); prbarID = (int)rd["prbarCorpID"]; }
                OdbcDataReader rdMens = new OdbcCommand("Select * from _1CBarCode where toPrbarID = " + prbarID.ToString(), myconn.conn).ExecuteReader();
                if (!rdMens.Read() || rewrite)
                {
                    if (rdMens.HasRows) nomen_mens = (Guid)rdMens["Guid_mens"]; else nomen_mens = Guid.NewGuid();
                    unit.ID = nomen_mens.ToString();
                    unit.OwnerID = nomen_guid.ToString();
                    unit.volume = (decimal)rd["PVolume"];
                    if (unit.volume == 0) unit.volume = 1;
                    unit.weight = (decimal)rd["GrossWeight"];
                    if (unit.weight == 0) unit.weight = 1;
                    unit.ClassifierID = "31";
                    unit.Name = rd["name"].ToString();
                    unit.coefficient = (decimal)rd["ScaleFactor"];
                    unit.BarCode = barcode;
                    unit.Code = prbarID.ToString();




                    if (rd["isNotBase"].ToString() == "0") unit.main = true; else unit.main = false;
                    res = ws1C.SetStorangeUnit(unit, "knit_ws");

                    if (!res) if (!res) throw new SyntaxErrorException("Не удалось записать штрих-код  PrbarID  " + prbarID.ToString());
                    if (!rdMens.HasRows)
                    {
                        OdbcCommand cmdSave = new OdbcCommand("Insert into _1CBarCode (toPrbarID,Guid_mens) values (" + prbarID.ToString() + " , '" + nomen_mens + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                        cmdSave.Dispose();
                    }
                }

            }
            rd.Close();
            rd = null;
            return nomen_guid;
        }


        static Guid GetSupplierID(int sid)
        {
            LogAdd("GetSupplierID(sid:" + sid.ToString() + ")");
            axContactInformation suppInfo = new axContactInformation();
            axContactInformation[] suppArr = new axContactInformation[0];
            axCounterparty suppl = new axCounterparty();
            Guid res;
            OdbcCommand cmd = new OdbcCommand("Select * from _1CSupplier where tosid = " + sid.ToString(), myconn.conn);
            OdbcDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr["guidWms"].ToString().Length == 0)
                {
                    res = Guid.NewGuid();
                    OdbcCommand cmdID = new OdbcCommand("Update _1CSupplier SET guidWms =  '" + res + "' where  tosid = " + sid.ToString(), myconn.conn);
                    cmdID.ExecuteNonQuery();
                }
                else res = (Guid)dr["guidWms"];
            }
            else
            {
                res = Guid.NewGuid();
                OdbcCommand cmdID = new OdbcCommand("Insert into _1CSupplier (tosid,guidWms) values (" + sid.ToString() + " , '" + res + "')", myconn.conn);
                cmdID.ExecuteNonQuery();
            }
            OdbcCommand cmdSupp = new OdbcCommand("Select * from supplier where sid = " + sid.ToString(), myconn.conn);
            OdbcDataReader rdSupp = cmdSupp.ExecuteReader();
            if (rdSupp.Read())
            {
                if ((int)rdSupp["YnGroup"] == 1)
                {
                    suppl.IsGroup = true;
                    suppl.Name = rdSupp["Name"].ToString();
                    suppl.GroupID = GetParentSupplierID(sid).ToString();
                    suppl.FullName = rdSupp["Name"].ToString();
                    suppl.ID = res.ToString();
                    suppl.Code = sid.ToString();
                }
                else
                {
                    suppl.IsGroup = false;
                    suppl.Name = rdSupp["Name"].ToString();
                    suppl.FullName = rdSupp["Name"].ToString();
                    suppl.LegalEntity = true;
                    suppl.ID = res.ToString();
                    suppl.GroupID = GetParentSupplierID(sid).ToString();
                    suppl.Code = sid.ToString();
                    Array.Resize<WebServ.axContactInformation>(ref suppArr, 1);
                    suppArr[0] = suppInfo;
                    suppl.ContactInformation = suppArr;
                }
                bool r = ws1C.SetCounterparty(suppl, "knit_ws");
                rdSupp.Close();
                rdSupp = null;
            }
            dr.Close();
            dr = null;
            cmd = null;
            return res;
        }


        static Guid GetSupplierIDCancellation(int sid) //вариант функции который работал в 1CGate_N 
        {
            LogAdd("GetSupplierIDCancellation(sid:" + sid.ToString() + ")");
            axContactInformation suppInfo = new axContactInformation();
            axContactInformation[] suppArr = new axContactInformation[0];
            axCounterparty suppl = new axCounterparty();
            Guid res;
            OdbcCommand cmd = new OdbcCommand("Select * from _1CSupplier where tosid = " + sid.ToString(), myconn.conn);
            OdbcDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                res = (Guid)dr["guidSid"];
            else
                res = Guid.NewGuid();

            OdbcCommand cmdID = new OdbcCommand("Insert into _1CSupplier (tosid,guidSid) values (" + sid.ToString() + " , '" + res + "')", myconn.conn);
            cmdID.ExecuteNonQuery();

            OdbcCommand cmdSupp = new OdbcCommand("Select * from supplier where sid = " + sid.ToString(), myconn.conn);
            OdbcDataReader rdSupp = cmdSupp.ExecuteReader();
            if (rdSupp.Read())
            {
                if ((int)rdSupp["YnGroup"] == 1)
                {
                    suppl.IsGroup = true;
                    suppl.Name = rdSupp["Name"].ToString();
                    suppl.GroupID = GetParentSupplierID(sid).ToString();
                    suppl.FullName = rdSupp["Name"].ToString();
                    suppl.ID = res.ToString();
                    suppl.Code = sid.ToString();
                }
                else
                {
                    suppl.IsGroup = false;
                    suppl.Name = rdSupp["Name"].ToString();
                    suppl.FullName = rdSupp["Name"].ToString();
                    suppl.LegalEntity = true;
                    suppl.ID = res.ToString();
                    suppl.GroupID = GetParentSupplierID(sid).ToString();
                    suppl.Code = sid.ToString();
                    Array.Resize<WebServ.axContactInformation>(ref suppArr, 1);
                    suppArr[0] = suppInfo;
                    suppl.ContactInformation = suppArr;
                }
                bool r = ws1C.SetCounterparty(suppl, "knit_ws");
                rdSupp.Close();
                rdSupp = null;
            }
            dr.Close();
            dr = null;
            cmd = null;
            return res;
        }


        static Guid ExportTree(int plid, bool rewrite = true) //выгрузка структуры каталога товаров 
        {
            LogAdd("ExportTree(plid:" + plid.ToString() + ")");
            if (plid == 0) return Guid.Empty;
            int parent;
            Guid nomen_guid;
            OdbcCommand cmd = new OdbcCommand("Select * from priceList where plid = " + plid.ToString(), myconn.conn);
            OdbcDataReader rd = cmd.ExecuteReader();
            WebServ.axNomenclature nm = new axNomenclature();
            if (rd.Read())
            {
                parent = (int)rd["parent"];

                OdbcCommand cmd1C = new OdbcCommand("Select * from _1CNomenclature where toplid = " + plid.ToString(), myconn.conn);
                OdbcDataReader rd1C = cmd1C.ExecuteReader();
                if (!rd1C.Read() || rewrite)
                {
                    if (rd1C.HasRows) nomen_guid = (Guid)rd1C["Guid_nom"]; else nomen_guid = Guid.NewGuid();
                    nm.ID = nomen_guid.ToString();
                    nm.IsGroup = true;
                    nm.FullName = rd["kind"].ToString();
                    nm.Name = rd["kind"].ToString();
                    nm.GroupID = ExportTree(parent).ToString();
                    bool res = ws1C.SetNomenclature(nm, "knit_ws");
                    if (!res) throw new SyntaxErrorException("Не удалось записать группу товаров  " + nm.Name + " ID=" + plid.ToString());
                    if (!rd1C.HasRows)
                    {
                        OdbcCommand cmdSave = new OdbcCommand("Insert into _1CNomenclature (toPlid,Guid_nom) values (" + plid.ToString() + " , '" + nomen_guid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                        cmdSave.Dispose();
                    }
                    rd1C.Close();
                    rd.Close();
                    return nomen_guid;
                }
                else
                {
                    Guid result = (Guid)rd1C["Guid_nom"];
                    rd1C.Close();
                    rd.Close();
                    return result;
                }
            }
            rd.Close();
            return Guid.Empty;
        }


        static void ExportGroupNomenclature(int plid) //выгрузка в 1С группы товаров 
        {
            LogAdd("ExportGroupNomenclature(plid: " + plid.ToString() + ")");
            OdbcCommand cmd = new OdbcCommand("Select * from pricelist where parent = " + plid.ToString(), myconn.conn);
            OdbcDataReader dt = cmd.ExecuteReader();
            while (dt.Read())
            {
                System.GC.Collect();
                if ((int)dt["ynGroup"] == 0)
                    ExportNomenclatureSimple((int)dt["plid"]);
                else
                    ExportGroupNomenclature((int)dt["plid"]);
            }
            dt.Close();
            dt = null;
        }


        static string GetWbGuid(string wbmid)
        {
            LogAdd("GetWbGuid(wbmid: " + wbmid + ")");
            OdbcCommand cmd = new OdbcCommand("select * from _1Cwbmain where towbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();
            string res = "";
            if (rdMain.Read())
            {
                res = rdMain["wbGuid"].ToString();
                rdMain.Close();
                rdMain = null;
                return res;
            }
            rdMain.Close();
            rdMain = null;
            Guid wbGuid = Guid.NewGuid();
            OdbcCommand cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + wbmid.ToString() + ",'" + wbGuid + "')", myconn.conn);
            cmdSave.ExecuteNonQuery();
            return wbGuid.ToString();
        }


        static string GetOrganizationID(int toFirmOfficeId)
        {
            LogAdd("GetOrganizationID(toFirmOfficeId: " + toFirmOfficeId.ToString() + ")");
            switch (toFirmOfficeId)
            {
                case 84115:
                    return "b14abe20-bb06-11e7-8109-74e6e2fa6c43"; // ООО «Солюшнс Принт»    
                case 124955:
                    return "c0811a58-bb06-11e7-8109-74e6e2fa6c43"; // ООО «Промшвейтех»    
                case 114912:
                    return "cf5e138d-bb06-11e7-8109-74e6e2fa6c43"; // ООО «ПромТехКомплект»    
                case 98626:
                    return "d8e36de5-bb06-11e7-8109-74e6e2fa6c43"; // ООО «Новые Технологии»    
                case 127196:
                    return "e5342c35-bb06-11e7-8109-74e6e2fa6c43"; // ООО «Импульс
                case 130007:
                    return "ee4d1f38-bb06-11e7-8109-74e6e2fa6c43"; // ООО «альфа
                case 130008:
                    return "0480e40e-bb07-11e7-8109-74e6e2fa6c43"; // ООО «комета
                case 131425:
                    return "0dc05bcf-bb07-11e7-8109-74e6e2fa6c43"; // ООО «черненко
                case 134293:
                    return "1a3e2e3a-bb07-11e7-8109-74e6e2fa6c43"; // ООО «славина
                case 148554:
                    return "242ee88b-bb07-11e7-8109-74e6e2fa6c43"; // ООО «клочан
                case 118039:
                    return "1b5e620d-d38d-11e4-b166-c860006e2886"; // ООО «К-Логистика
            }
            return "1b5e620d-d38d-11e4-b166-c860006e2886";
        }


        static Guid GetMeansBaseGuid(int plid)//получаем ссылку на ед.измерения для товара
        {
            LogAdd("GetMeansBaseGuid(plid: " + plid.ToString() + ")");
            Guid res;
            OdbcCommand cmd = new OdbcCommand("Select * from _1CBarCode inner join pricelist_barcode on toPrbarID = PrBarId where toplid =" + plid.ToString() + " and toMeasid = (Select top 1 toBaseMeasId from measures m inner join pricelist p on m.measid=p.tomeasgroupid and plid =  " + plid.ToString() + ")", myconn.conn);
            OdbcDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
                res = (Guid)rd["Guid_mens"];
            else
                res = Guid.Empty;
            rd.Close();
            rd = null;
            return res;
        }


        static Guid GetParentSupplierID(int sid)
        {
            LogAdd("GetParentSupplierID(sid: " + sid.ToString() + ")");
            Guid res = Guid.Empty;
            int parent;
            OdbcCommand cmd = new OdbcCommand("Select * from supplier where sid = " + sid.ToString(), myconn.conn);
            OdbcDataReader drTree = cmd.ExecuteReader();
            while (drTree.Read())
            {
                parent = (int)drTree["parent"];
                if (parent > 0) res = GetSupplierID(parent);
            }
            drTree.Close();
            drTree = null;
            cmd = null;
            return res;
        }


        static bool isCanExport(int plid)
        {
            LogAdd("isCanExport(plid: " + plid.ToString() + ")");
            return true;
            OdbcCommand cmd = new OdbcCommand("Select  IsGoodInGroup( " + plid.ToString() + " , 6746 ) as p", myconn.conn);
            int res = (int)cmd.ExecuteScalar();
            if (res == 1) return true;
            cmd = new OdbcCommand("Select  IsGoodInGroup( " + plid.ToString() + " , 6762 ) as p", myconn.conn);
            res = (int)cmd.ExecuteScalar();
            if (res == 1) return true;
            return false;
        }


        static Guid GetUidWb(int wbmid) //получить/задать GUID для накладной
        {
            LogAdd("GetUidWb(wbmid: " + wbmid.ToString() + ")");
            Guid res = Guid.Empty;
            if (wbmid == 0) return res;
            OdbcCommand cmd = new OdbcCommand("Select * from _1CWbMain where towbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                res = (Guid)dr["wbGuid"];
            }
            else
            {
                res = Guid.NewGuid();
                OdbcCommand cmdID = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + wbmid.ToString() + " , '" + res + "')", myconn.conn);
                cmdID.ExecuteNonQuery();
            }
            dr.Close();
            dr = null;
            return res;
        }
        
        
        static string GetNameToPlid(int plid)
        {
            string res = "";
            OdbcDataReader rdPrList = new OdbcCommand("Select  isnull((select name  from goodname where gnid = tognid), '') + ' ' + 	isnull( (select name from firms where tofirmid = Firmid ), '' ) +  ' ' + kind	as FullName  from pricelist where plid = " + plid.ToString(), myconn.conn).ExecuteReader();
            if (rdPrList.Read())
            {
                res = rdPrList["FullName"].ToString();
            }
            rdPrList.Close();
            rdPrList = null;
            return res;
        }

        #endregion


        static void ExportInvoiceGross(int wbmid) //выгрузка по грузовым местам
        {
            Console.WriteLine("Выгрузка в 1С по грузовым местам: " + wbmid.ToString());
            Guid wbGuid;
            OdbcCommand cmd = new OdbcCommand("Select wbmain.*  , wbGuid, isnull(_1cwbmain.towbmid,0) as _1ctowbmid from wbmain  left join _1cwbmain on wbmid = towbmid where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();
            WebServ.axArrivalPlan plan = new axArrivalPlan();
            WebServ.axGoodsLine gl = null;
            WebServ.axGoodsLine[] glArr = new axGoodsLine[0];
            if (rdMain.Read())
            {
                if ((int)rdMain["_1ctowbmid"] == 0) wbGuid = Guid.NewGuid(); else wbGuid = (Guid)rdMain["wbGuid"];
                plan.ID = wbGuid.ToString();
                plan.Date = (DateTime)rdMain["chDate"];
                plan.ExpectedDate = (DateTime)rdMain["planknitindate"];
                plan.CounterpartyID = GetSupplierID((int)rdMain["tosupplierid"]).ToString();
                plan.Number = rdMain["WbNum"].ToString();

                plan.CargoSpace = true;
                OdbcCommand cmdDet = new OdbcCommand("Select wbdet.* , isnull(_1c.toplid,0) as _1cToplid , Guid_nom  from wbdet left join _1CNomenclature _1c on wbdet.toplid = _1c.toplid  where towbmid = " + wbmid.ToString(), myconn.conn);
                OdbcDataReader rdDet = cmdDet.ExecuteReader();
                int cntRow = 0;
                while (rdDet.Read())
                {
                    cntRow++;
                    gl = null;
                    gl = new axGoodsLine();
                    gl.NomenclatureID = "55";
                    gl.Quantity = (decimal)rdDet["numberdistr"];
                    gl.QuantityUnit = (decimal)rdDet["numberdistr"];
                    //gl.aShippingPlanID = "5f39fddc-9223-4316-b100-6b5a78985ecf";
                    //gl.Quantity = 55;
                    //gl.QuantityUnit = 55;
                    //gl.StorangeUnitID = GetMeansBaseGuid((int)rdDet["toplid"]).ToString();
                    //gl.LineNum = Convert.ToDecimal(rdDet["wbdetid"]);
                    //gl.Amount = (decimal)rdDet["prUnit"];

                    Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                    glArr[cntRow - 1] = gl;
                }
                rdDet.Close();
                rdDet = null;
                plan.GoodsLine = glArr;
                bool res = ws1C.SetArrivalPlan(plan, "knit_ws");
                if (res)
                {
                    OdbcCommand cmdSave;
                    if ((int)rdMain["_1ctowbmid"] == 0)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + rdMain["wbmid"].ToString() + ",'" + wbGuid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                    }
                }
            }
            rdMain.Close();
            rdMain = null;
        }


        static void ExportShippingGross(int wbmid) //выгрузка в 1С накладной на отгрузку
        {
            Console.WriteLine("Выгрузка в 1С поставки : " + wbmid.ToString());
            Guid wbGuid;
            OdbcCommand cmd = new OdbcCommand("Select wbmain.*  , wbGuid, isnull(_1cwbmain.towbmid,0) as _1ctowbmid from wbmain  left join _1cwbmain on wbmid = towbmid where wbmid = " + wbmid.ToString(), myconn.conn);
            OdbcDataReader rdMain = cmd.ExecuteReader();
            WebServ.axShippingPlan plan = new axShippingPlan();
            WebServ.axGoodsLine gl = null;
            WebServ.axGoodsLine[] glArr = new axGoodsLine[0];
            if (rdMain.Read())
            {
                if ((int)rdMain["_1ctowbmid"] == 0) wbGuid = Guid.NewGuid(); else wbGuid = (Guid)rdMain["wbGuid"];
                plan.ID = wbGuid.ToString();
                plan.Date = (DateTime)rdMain["DateControl"];
                plan.ExpectedDate = (DateTime)rdMain["DateControl"];
                plan.CounterpartyID = plan.CounterpartyID = GetSupplierID((int)rdMain["tosupplierid"]).ToString();
                plan.Number = rdMain["WbNum"].ToString();
                OdbcCommand cmdDet = new OdbcCommand("Select wbdet.* , isnull(_1c.toplid,0) as _1cToplid , Guid_nom  from wbdet left join _1CNomenclature _1c on wbdet.toplid = _1c.toplid  where towbmid = " + wbmid.ToString(), myconn.conn);
                OdbcDataReader rdDet = cmdDet.ExecuteReader();
                int cntRow = 0;
                while (rdDet.Read())
                {
                    cntRow++;
                    Guid nomenGuid;
                    if ((int)rdDet["_1cToplid"] == 0)
                        nomenGuid = ExportNomenclatureSimple((int)rdDet["Toplid"], true);
                    else
                        nomenGuid = (Guid)rdDet["Guid_nom"];
                    gl = null;
                    gl = new axGoodsLine();
                    gl.NomenclatureID = "55";
                    gl.Quantity = (decimal)rdDet["numberdistr"];
                    gl.QuantityUnit = (decimal)rdDet["numberdistr"];
                    //gl.Quantity = 22;
                    //gl.QuantityUnit = 22;
                    //                    gl.StorangeUnitID = GetMeansBaseGuid((int)rdDet["toplid"]).ToString();
                    //gl.LineNum = Convert.ToDecimal(rdDet["wbdetid"]);
                    //gl.Amount = (decimal)rdDet["prUnit"];
                    Array.Resize<WebServ.axGoodsLine>(ref glArr, cntRow);
                    glArr[cntRow - 1] = gl;
                }
                rdDet.Close();
                rdDet = null;
                plan.GoodsLine = glArr;
                bool res = ws1C.SetShippingPlan(plan, "knit_ws");
                if (res)
                {
                    OdbcCommand cmdSave;
                    if ((int)rdMain["_1ctowbmid"] == 0)
                    {
                        cmdSave = new OdbcCommand("Insert into _1CWbMain (towbmid,wbGuid) values (" + rdMain["wbmid"].ToString() + ",'" + wbGuid + "')", myconn.conn);
                        cmdSave.ExecuteNonQuery();
                    }
                }

            }
            rdMain.Close();
            rdMain = null;
        }


        static string digsOnly(string str)
        {
            Regex reg = new Regex("[0-9]+");
            MatchCollection mc = reg.Matches(str);
            StringBuilder sb = new StringBuilder();
            foreach (Match matc in mc)
                sb.Append(matc.Value);
            return sb.ToString();
        }


        static void LogAdd(string logstr) //вывод лога в таблицу _1Clog БД
        {
            try
            {
                System.Threading.Thread.Sleep(100);
                Console.WriteLine(logstr);
                if (idQueue > 0)
                {
                    OdbcCommand cmd = new OdbcCommand("Insert into _1Clog (toIdQueue,MsgLog) values (" + idQueue.ToString() + ",'" + logstr + "')", myconn.conn);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }
                else
                {
                    OdbcCommand cmd = new OdbcCommand("Insert into _1Clog (toIdQueue,MsgLog) values (" + idQueue.ToString() + ",'" + logstr + "')", myconn.conn);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка в процедуре LogAdd");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
