//create bill
Guid UserId = (Guid)Session["userID"];
var u = db.users.Find(UserId);

string colId = u.CollectionId;
Bill bill = new Bill();

string callback_url = "YourcallbackURL";

WebRequest req = WebRequest.Create(@"https://www.billplz.com/api/v3/bills?collection_id="+colId+"&email="+u.Email+"&mobile="+u.PhoneNumber+"&name="+u.FirstName+" "+u.LastName+"&amount="
  +selectedamount+ "&callback_url="+callback_url+ "&description=Your transaction description");

req.Method = "POST";
req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("YourSecretKey"));

try
{
    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

    if (resp.StatusCode == HttpStatusCode.OK)
    {
        // Read the response body as string
        Stream dataStream = resp.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        data = reader.ReadToEnd();
        bill = JsonConvert.DeserializeObject<Bill>(data);
       
        resp.Close();

        //redirect user to billplz website for payment
        return Redirect(bill.Url);
    }

}
catch (Exception ex)
{
    throw new Exception(ex.Message);
}


