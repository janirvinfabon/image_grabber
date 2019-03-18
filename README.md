# image_grabber
A dot net application that download an image using the URL from user. This application uses System.Net namespace

# source class

public class link_sources
    {
        public int No { get; set; }
        [DisplayName("Image Source")] public string ImageSource { get; set; }
        [Browsable(false)] public string ImageExtension { get; set; }
    }

# main form properties
private WebBrowser browser = new WebBrowser(); // add a web browser control

private string defaultLocation = string.Empty;
private string linkName = string.Empty;

# get all img tag
var counter = 1;
var tmpList = new List<link_sources>();

foreach (HtmlElement item in this.browser.Document.GetElementsByTagName("IMG"))
{
	var uri = new Uri(item.GetAttribute("src"));
	if (!string.IsNullOrEmpty(Path.GetExtension(uri.AbsolutePath)))
	{
		var src = item.GetAttribute("src");
		tmpList.Add(new link_sources() { No = counter++, ImageSource = src, ImageExtension =  src.Split('.').Last().ToLower() });
	}
}

# download each file
src_list.ForEach((x) =>
{
	var link = x.ImageSource;
	var name = x.ImageSource.Split('/').Last().Replace("%", "");
	var file = string.Format(@"{0}\{1}\{2}", this.defaultLocation, this.linkName, name);

	try
	{
		this.check_folder(file);                            // create a seperate directory for each link
		new WebClient().DownloadFile(link, file);           // download the file
		total++;
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
	}
});


# check file dir
var dir = Path.GetDirectoryName(file);
if (!Directory.Exists(dir))
{
	Directory.CreateDirectory(dir);
}
