
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Assimil
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<DVLotteryCheck> list = StatusCheckProvider.Get();
            //var updDV = list.Where(e => !(e.Status == "Issued" || e.Status == "Refused")).Take(50);
            DateTime now = DateTime.Now;
            
            DataCenter data = new DataCenter();

            List<DVLotteryUser> list = data.GetList("AF").CreateListFromTable<DVLotteryUser>();

            // list = list.Where(e => e.UpdateDt == null).Skip(20000).Take(50).ToList();
            // list = list.Where(e => e.UpdateDt == null || (e.UpdateDt != null && e.UpdateDt.AddDays(7).Date < now.Date)).Take(2000).ToList();
            list = list.Where(e => (e.UpdateDt != null && e.UpdateDt.Date < now.AddDays(-0.5).Date)).Take(3000).ToList();
            // list = list.Where(e => e.FCN == "2015AF81").ToList();
            List<DVLotteryUser> updlist = new List<DVLotteryUser>();

            List<DVLotteryUser> AtNVClist = new List<DVLotteryUser>();
            List<DVLotteryUser> transitlist = new List<DVLotteryUser>();

            string url = @"https://ceac.state.gov/CEACStatTracker/Status.aspx?App=IV";  // 2015EU18

            string formData = "ctl00%24ToolkitScriptManager1=ctl00%24ContentPlaceHolder1%24UpdatePanel1%7Cctl00%24ContentPlaceHolder1%24btnSubmit&ctl00_ToolkitScriptManager1_HiddenField=%3B%3BAjaxControlToolkit%2C%20Version%3D3.5.51116.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3D28f01b0e84b6d53e%3Aen-US%3A2a06c7e2-728e-4b15-83d6-9b269fb7261e%3Ade1feab2%3Af2c8e708%3A8613aea7%3Af9cec9bc%3A3202a5a2%3Aa67c2700%3A720a52bf%3A589eaa30%3Aab09e3fe%3A87104b7c%3Abe6fb298&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPaA8FDzhkMjBjZTUxNmZjMDg3YhgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAQUjY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRidG5TdWJtaXQgc9l%2FZnhYXZAK0P5Q6gb0jYgpww%3D%3D&__EVENTVALIDATION=%2FwEWBwLYrfmkDgK654HdAwK7xZyzCgLcqs7DBgLF9PH2AgLmwPTHBAL40JWiChgAtqdj2kr%2FkyJHeZ%2FW%2BXY0ypPn&ctl00%24ContentPlaceHolder1%24ddlApplications=IV&ctl00%24ContentPlaceHolder1%24txbCase=****&__ASYNCPOST=true&ctl00%24ContentPlaceHolder1%24btnSubmit.x=70&ctl00%24ContentPlaceHolder1%24btnSubmit.y=16";

            foreach (var cand in list)
            {
                string htmlText = XmlHttpRequest(url, formData.Replace("****", cand.FCN));

                // test HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlText);
                int issued = 0, ready = 0, transit = 0, refused = 0, ap = 0;

                // Get Status
                var NewStatusNode = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_ucApplicationStatusView_lblStatus']");
                string newStatus = NewStatusNode.InnerHtml;

                if (newStatus == "In Transit") 
                {
                    // update Status
                    cand.Status = newStatus;
                    transitlist.Add(cand);
                }
                else  if (newStatus != "At NVC")
                {
                    // update Status
                    cand.Status = newStatus;

                    switch (newStatus)
                    {
                        case "Issued":
                            issued++;
                            break;
                        case "Ready":
                            ready++;
                            break;
                        case "Refused":
                            refused++;
                            break;
                        case "Administrative Processing":
                            ap++;
                            break;
                        case "In Transit":
                            transit++;
                            break;
                        default:
                            break;
                    }

                    var CaseNoWhereNode = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_ucApplicationStatusView_lblCaseNo']");
                    string CaseNoWhereValue = CaseNoWhereNode.InnerHtml;

                    // Get CON
                    string newCON = (CaseNoWhereValue.Length > 2) ? CaseNoWhereValue.Substring(CaseNoWhereValue.Length - 3, 3) : CaseNoWhereValue;

                    // update CON
                    cand.CON = newCON;

                    // Get SubmitDate
                    var SubmitDateNode = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_ucApplicationStatusView_lblSubmitDate']");
                    string newSubmitDate = SubmitDateNode.InnerHtml;

                    // update SubmitDate
                    cand.SubmitDate = Convert.ToDateTime(newSubmitDate);

                    // Get StatusDate
                    var StatusDateNode = doc.DocumentNode.SelectSingleNode("//span[@id='ctl00_ContentPlaceHolder1_ucApplicationStatusView_lblStatusDate']");
                    string newStatusDate = StatusDateNode.InnerHtml;

                    // update StatusDate
                    cand.StatusDate = Convert.ToDateTime(newStatusDate);

                    var FamilyWhereNode = doc.DocumentNode.SelectSingleNode("//div[@id='ctl00_ContentPlaceHolder1_ucApplicationStatusView_ucApplicationList_UpdatePanel1']");

                    int countPerson = 0;

                    if (FamilyWhereNode != null)
                    {
                        string currentMember;

                        for (int f = 1; f < 8; f++)
                        {
                            currentMember = "ctl00_ContentPlaceHolder1_ucApplicationStatusView_ucApplicationList_gridCases_ctl0" + f + "_lnkView";
                            var searchPerson = doc.DocumentNode.SelectSingleNode("//a[@id='" + currentMember + "']");

                            if (searchPerson == null) break;

                            countPerson++;
                        }
                    }

                    // Get FamilyNumbers
                    int newFamilyNumbers = (countPerson == 0) ? 1 : countPerson;

                    // update FamilyMembers
                    cand.FamilyMembers = newFamilyNumbers;

                    if (countPerson != 0)
                    {
                        issued = ready = transit = refused = ap = 0;

                        var allElementsWithClassStatus = doc.DocumentNode.SelectNodes("//td[contains(@class,'status')]");

                        foreach (var elt in allElementsWithClassStatus)
                        {
                            string perStatus = elt.InnerHtml.Replace("<div>", "").Replace("</div>", "").Replace("\r\n", "").Trim();

                            switch (perStatus)
                            {
                                case "Issued":
                                    issued++;
                                    break;
                                case "Ready":
                                    ready++;
                                    break;
                                case "Refused":
                                    refused++;
                                    break;
                                case "Administrative Processing":
                                    ap++;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // 

                    cand.ISSUED = issued;
                    cand.READY = ready;
                    cand.REFUSED = refused;
                    cand.AP = ap;
                    cand.TRANSFER = transit;

                    updlist.Add(cand);
                }
                else
                {
                    AtNVClist.Add(cand);
                }

            }

            foreach (var item in updlist)
            {
                data.updList(item);
            }

            foreach (var item in AtNVClist)
            {
                data.AtNVCList(item);
            }

            foreach (var item in transitlist)
            {
                data.TransitList(item);
            }

            Console.WriteLine("The END");
            Console.ReadKey();
        }

        public static string XmlHttpRequest(string urlString, string xmlContent)
        {
            string response = null;
            HttpWebRequest httpWebRequest = null;//Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse httpWebResponse = null;//Declare an HTTP-specific implementation of the WebResponse class

            //Creates an HttpWebRequest for the specified URL.
            httpWebRequest = (HttpWebRequest)WebRequest.Create(urlString);

            try
            {
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlContent);
                //Set HttpWebRequest properties
                httpWebRequest.Method = "POST";
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.111 Safari/537.36";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    //Writes a sequence of bytes to the current stream 
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();//Close stream
                }

                //Sends the HttpWebRequest, and waits for a response.
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream into StreamReader
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                            response = reader.ReadToEnd();
                    }
                }
                httpWebResponse.Close();//Close HttpWebResponse
            }
            catch (WebException we)
            {   //TODO: Add custom exception handling
                throw new Exception(we.Message);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                httpWebResponse.Close();
                //Release objects
                httpWebResponse = null;
                httpWebRequest = null;
            }
            return response;
        }

    }
}
