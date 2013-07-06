using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using myxslxml;

namespace MvcApplication2.Models
{
    public class MySGExcel
    {
        public  string _path = string.Empty;
        private CG.MyNamespace33.root_wb_pf _input33 = null;
        private CG.MyNamespace.root_wb_pf _input = null;

        private const string _root             = @"~/App_Data";
        //private const string _efnDefault = "01RFishheads.xls";

        //System.Globalization.CultureInfo  nfi = new System.Globalization.CultureInfo("nl-NL");
        System.Globalization.CultureInfo _nfi = new System.Globalization.CultureInfo("en-US");

        private string setWorksheet33(SpreadsheetGear.IWorkbook workbook)
        {
            string retval = string.Empty;

            // for each input work sheet
            for (int iii = 0; iii < _input33.Items.Count(); iii++)
            {
                if (_input33.Items[iii].GetType() == typeof(CG.MyNamespace33.root_wb_pfInput))
                {
                    CG.MyNamespace33.root_wb_pfInput input = (CG.MyNamespace33.root_wb_pfInput)_input33.Items[iii];
                    // populate sheet(s).

                    string name = string.Empty;
                    name = ((CG.MyNamespace33.root_wb_pfInput)(_input33.Items[iii])).name;

                    SpreadsheetGear.IWorksheet ws = workbook.Worksheets[name];

                    for (int rrr = 0; rrr < input.CELL.Length; rrr++)
                    {
                        string sCell = input.CELL[rrr].Cell;
                        ws.Cells[sCell].Value = input.CELL[rrr].Value;
                    }
                }
            }

            return retval;
        }


        private string setWorksheet(SpreadsheetGear.IWorksheet ws)
        {
            // validate
            // use _input

            string retval = string.Empty;
            
            for (int iii = 0; iii < _input.CELL.Length; iii++)
            {
                string sCell = _input.CELL[iii].Cell;
                ws.Cells[sCell].Value = _input.CELL[iii].Value;
            }

            return retval;
        }


        private string getWorksheet33(SpreadsheetGear.IWorkbook workbook)
        {
            string retval = string.Empty;
            StringBuilder sb = new StringBuilder();

            // for each output work sheet
            for (int iii = 0; iii < _input33.Items.Count(); iii++)
            {
                if (_input33.Items[iii].GetType() == typeof(CG.MyNamespace33.root_wb_pfOutput))
                {
                    CG.MyNamespace33.root_wb_pfOutput output = (CG.MyNamespace33.root_wb_pfOutput)_input33.Items[iii];

                    string name = string.Empty;
                    name = output.name;
                    string outputRange = string.Empty;
                    SpreadsheetGear.IWorksheet ws = workbook.Worksheets[name];
                    // if null it might be a chart.
                    if (ws == null)
                    { 
                        // get chart data
                        SpreadsheetGear.IWorkbookSet workbookSet = workbook.WorkbookSet;
                        SpreadsheetGear.IChartSheet ichart = 
                                          (SpreadsheetGear.IChartSheet)workbookSet.Workbooks[1].Sheets[name];
                        string szChart = GetChart22(ichart);

                        string strName = string.Format("<{0}>{1}</{0}>", "name", ichart.Name);

                        StringBuilder sb33 = new StringBuilder();

                        sb33.Append("<results>");
                        sb33.Append(szChart);
                        sb33.Append(strName);
                        sb33.Append("</results>");

                        outputRange = sb33.ToString();


                    }
                    else
                    {
                        // retrieve sheet(s).
                        outputRange = getWorksheet02(ws, output);
                    }
                    sb.Append(outputRange);
                }
            }

            string fn = Path.GetFileName(this._path);
            string tagfname = string.Format("<filename>{0}</filename>", fn);


            StringBuilder sb22 = new StringBuilder();
            sb22.Append("<root_wb_pf>");
            sb22.Append(tagfname);
            sb22.Append(sb);
            sb22.Append("</root_wb_pf>");

            retval = sb22.ToString();
            return retval;
        }

        string getWorksheet02(SpreadsheetGear.IWorksheet ws, CG.MyNamespace33.root_wb_pfOutput src)
        {
            string retval = string.Empty;
            // check
            if (ws == null || src == null)
            {
                return retval;
            }

            StringBuilder sb = new StringBuilder();

            // for each cell in the output
            for (int iii = 0; iii < src.CELL.Count(); iii++)
            {
                string ccc = src.CELL[iii].Cell;
                string strCell = string.Format("<{0}>{1}</{0}>", "Cell", ccc);
                string Value = (ws.Cells[ccc].Value == null) ? "" : ws.Cells[ccc].Value.ToString().Trim();

                string strVal = string.Format("<{0}>{1}</{0}>", "Value", Value.Trim());

                string strElement = string.Format("<{0}>{1}</{0}>", "CELL", strCell + strVal );

                sb.Append(strElement);
            }

            string strName = string.Format("<{0}>{1}</{0}>", "name", ws.Name);

            StringBuilder sb22 = new StringBuilder();

            sb22.Append("<results>");
            sb22.Append(sb);
            sb22.Append(strName);
            sb22.Append("</results>");

            retval = sb22.ToString();

            return retval;
        }
       
        private string getWorksheet(SpreadsheetGear.IWorksheet ws)
        {
            string retval = string.Empty;
            //ws.UsedRange;
            int  rows = ws.UsedRange.RowCount;
            long cols = ws.UsedRange.ColumnCount;
            StringBuilder sb = new StringBuilder();

            SpreadsheetGear.IWorkbook workbook = ws.Workbook;

            // define range to retrieve.
            string[] cells = new string[] { "$D87", "F87", "G87", "H87" 
                                           ,"$D88", "F88", "G88", "H88",  };

            foreach (string ccc in cells)
            {
                string strCell = string.Format("<{0}>{1}</{0}>", "Cell", ccc);

                string Value = (ws.Cells[ccc].Value == null) ? "" : ws.Cells[ccc].Value.ToString().Trim();
                //Value = string.Format(_nfi, "{0,30:C0}", Value);

                string strVal = string.Format("<{0}>{1}</{0}>", "Value", Value.Trim());

                string strElement = string.Format("<{0}>{1}</{0}>", "CELL", strCell + strVal);

                sb.Append(strElement);
            }

            string strName = string.Format("<{0}>{1}</{0}>", "name", ws.Name);

            StringBuilder sb22 = new StringBuilder();
            sb22.Append("<root_wb_pf>");
            sb22.Append(sb);
            sb22.Append(strName);
            sb22.Append("</root_wb_pf>");

            retval = sb22.ToString();

            return retval;    
        }


        private string GetResults33()
        {
            SpreadsheetGear.IWorkbook workbook = null;
            try
            {
                // validate

                SpreadsheetGear.IWorkbookSet workbookSet = SpreadsheetGear.Factory.GetWorkbookSet();
                workbook = workbookSet.Workbooks.Add();

                workbook = workbookSet.Workbooks.Open(_path);

                string results = setWorksheet33(workbook);

                string sr = getWorksheet33(workbook);
                return sr;
            }
            catch (Exception eee)
            {
                string msg = eee.Message;
                return (eee.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
            }
        }
        //private string GetResults()
        //{
        //    SpreadsheetGear.IWorkbook workbook = null;
        //    try
        //    {
        //        // validate

        //        SpreadsheetGear.IWorkbookSet workbookSet = SpreadsheetGear.Factory.GetWorkbookSet();
        //        workbook = workbookSet.Workbooks.Add();

        //        workbook = workbookSet.Workbooks.Open(_path);

        //        string sss = setWorksheet(workbook.Worksheets[_input.name]);

        //        string sr = getWorksheet(workbook.Worksheets["Printer friendly"]);
        //        return sr;
        //    }
        //    catch (Exception eee)
        //    {
        //        string msg = eee.Message;
        //        return (eee.Message);
        //    }
        //    finally
        //    {
        //        if (workbook != null)
        //        {
        //            workbook.Close();
        //        }
        //    }
        //}


        static private string helper_ResultsAsXml(CG.MyNamespace.root_wb_pf src)
        {
            string retval = string.Empty;

            return retval;
        }


        static string helper_GetxxxPath(string xxx, string root, string fn)
        {
            string retval = string.Empty;

            string theServerFN = Path.Combine(root, fn);
            theServerFN = theServerFN.Replace("\\", "/");

            string physicalPath = System.Web.HttpContext.Current.Server.MapPath(theServerFN);

            if (File.Exists(physicalPath))
            {
                if (string.Compare(xxx.ToUpper(), "SERVER") == 0)
                {
                    retval = theServerFN;
                }
                else
                {
                    retval = physicalPath;
                }
            }

            return retval;
        }

        static public string runCalculation(string xml)
        {
            // Validate

            // Populate workbook.
            string fp = string.Empty;
            MySGExcel gg = new MySGExcel();
//            gg._path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/01RFishheads.xls");

            CG.MyNamespace.root_wb_pf bbb = MyXml.Deserialize<CG.MyNamespace.root_wb_pf>(xml);
            CG.MyNamespace33.root_wb_pf in33 = MyXml.Deserialize<CG.MyNamespace33.root_wb_pf>(xml);

            string root = _root;

            //string theServerEfn = Path.Combine(root, _efnDefault);
            string theEfn = string.Empty; //_efnDefault;

            var item = in33.Items.FirstOrDefault(i => i.GetType() == typeof(CG.MyNamespace33.root_wb_pfFile));
            if (item != null) 
            {
                string fu = ((CG.MyNamespace33.root_wb_pfFile)(item)).name;

                fp = helper_GetxxxPath("PHYSICAL", root, fu);

                if(!string.IsNullOrEmpty(fp))
                {
                    theEfn = fu;
                }
            }

            fp = helper_GetxxxPath("SERVER", root, theEfn);
            if (string.IsNullOrEmpty(fp))
            {
                string msg = string.Format("<error>excel file({0}) not found.</error>", 
                                            Path.GetFileName(theEfn));
                return msg;
            }

            gg._path = helper_GetxxxPath("PHYSICAL", _root, theEfn);

            gg._input33 = in33;
            gg._input = bbb;

            string results = gg.GetResults33();

            //string retXml = helper_ResultsAsXml(bbb);
            return results;
        }

        private string GetChart22(SpreadsheetGear.IChartSheet ichart)
        {
            string retval = string.Empty;

            try
            {
                SpreadsheetGear.Drawing.Image image21 = new SpreadsheetGear.Drawing.Image(ichart.Chart);

                using (System.Drawing.Bitmap bitmap = image21.GetBitmap())
                {
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        //memoryStream.WriteTo(Response.OutputStream);
                        //return bitmap;
                        retval = MyBitmap.WriteXML(bitmap, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }

                //return retval;
            }
            catch (Exception eee)
            {
                string msg = eee.Message;
                return null;
                //return (eee.Message);
            }
            return retval;
        }


        private System.Drawing.Bitmap GetChart()
        {
            SpreadsheetGear.IWorkbook workbook = null;
            try
            {
                string retval = string.Empty;
                // validate

                SpreadsheetGear.IWorkbookSet workbookSet = SpreadsheetGear.Factory.GetWorkbookSet();
                workbook = workbookSet.Workbooks.Add();

                workbook = workbookSet.Workbooks.Open(_path);

                SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[_input.name];
                
                //SpreadsheetGear.IRange cells =  workbookSet.Workbooks[1].Sheets[3].Workbook.Worksheets[0].Cells;
                // Create the image class from a specified range.
                //SpreadsheetGear.Drawing.Image image = new SpreadsheetGear.Drawing.Image(cells["A1:E16"]);

                    //new SpreadsheetGear.Charts.IChart;
                //SpreadsheetGear.IChartSheet ichart = (SpreadsheetGear.IChartSheet)workbookSet.Workbooks[1].Sheets["Survival rate"];
                SpreadsheetGear.IChartSheet ichart = 
                                          (SpreadsheetGear.IChartSheet)workbookSet.Workbooks[1].Sheets[_input.name];
                //workbookSet.Workbooks[1].Sheets["Survival rate"].Name
                //ichart.Chart

                SpreadsheetGear.Drawing.Image image21 = new SpreadsheetGear.Drawing.Image(ichart.Chart);
                //SpreadsheetGear.Drawing.Image image22 = new SpreadsheetGear.Drawing.Image(chart02);

                // Get a new bitmap image of the represented range.
                //using ()
                    
                //System.Drawing.Bitmap bitmap = image.GetBitmap();
                System.Drawing.Bitmap bitmap = image21.GetBitmap();
                {
                    // Stream the image to the client in PNG format.
                    
                    //Response.Clear();
                    //Response.ContentType = "image/png";
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    //memoryStream.WriteTo(Response.OutputStream);
                    return bitmap;
                }

                //return retval;
            }
            catch (Exception eee)
            {
                string msg = eee.Message;
                return null;
                //return (eee.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
            }
        }


        private System.Drawing.Bitmap GetChartByName(string chartname)
        {
            SpreadsheetGear.IWorkbook workbook = null;
            try
            {
                string retval = string.Empty;
                // validate

                SpreadsheetGear.IWorkbookSet workbookSet = SpreadsheetGear.Factory.GetWorkbookSet();
                workbook = workbookSet.Workbooks.Add();

                workbook = workbookSet.Workbooks.Open(_path);

                _input.name = "Printer friendly";
                SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[_input.name];

                SpreadsheetGear.Drawing.Image image21 = new SpreadsheetGear.Drawing.Image(worksheet.Shapes[chartname]);
                

                // Get a new bitmap image of the represented range.
                //using ()

                //System.Drawing.Bitmap bitmap = image.GetBitmap();
                System.Drawing.Bitmap bitmap = image21.GetBitmap();
                {
                    // Stream the image to the client in PNG format.

                    //Response.Clear();
                    //Response.ContentType = "image/png";
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    //memoryStream.WriteTo(Response.OutputStream);
                    return bitmap;
                }

                //return retval;
            }
            catch (Exception eee)
            {
                string msg = eee.Message;
                return null;
                //return (eee.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
            }
        }


        static public System.Drawing.Bitmap TEST_getChart(string xml)
        {
            // Validate

            //http://www.spreadsheetgear.com/support/samples/asp.net.sample.aspx?sample=imagesimplechart
            //CG.MyNamespace.root_wb_pf bbb = MyXml.Deserialize<CG.MyNamespace.root_wb_pf>(xml);

            MySGExcel gg = new MySGExcel();
            gg._path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/01RFishheads.xls");
           
            gg._input = new CG.MyNamespace.root_wb_pf();
            gg._input.name = "Stock value pr kg"; //"Survival rate";

            System.Drawing.Bitmap img = null;
            //if (String. Compare(xml, "chart21")==0)
            if (xml.IndexOf("chart", 0) != -1)
            {
                img = gg.GetChartByName(xml);
            }
            else
            {
                img = gg.GetChart();
                //System.Drawing.Bitmap img = gg.GetChart();
            }

            //string retXml = helper_ResultsAsXml(bbb);
            return img;
        }
    }
}