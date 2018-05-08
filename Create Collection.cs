//create a collection
Guid id = (Guid)Session["userID"]; //get the logged in user id
//get user record to check if he has a collection id or not
//in user table you should have a field called CollectionId
var u = db.users.Find(id); 
string colId = u.CollectionId;
if(colId == null ||  colId == String.Empty) //check if the user has a collection id or not
{
	Collection collection = new Collection();
	string title = "Put your title here";
	
	//call the api to create collection id
	WebRequest req = WebRequest.Create(@"https://www.billplz.com/api/v3/collections?title="+title);
	req.Method = "POST";
	req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("YourSecretKeyHere"));
	HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
	                    

	if(resp.StatusCode == HttpStatusCode.OK)
	{
		// Read the response body as string
		Stream dataStream = resp.GetResponseStream();
		StreamReader reader = new StreamReader(dataStream);
		data = reader.ReadToEnd();
		collection = JsonConvert.DeserializeObject<Collection>(data);
		u.CollectionId = collection.Id;
		db.SaveChanges();

		resp.Close();

		return View();

	}
}
else
{
    return View();

}