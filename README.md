# image_grabber
A dot net application that download an image using the URL from user. This application uses System.Net namespace.
This also uses a Thread, List<T> and linq

## source class 
```csharp
	public class link_sources
	{
		public int No { get; set; }
		[DisplayName("Image Source")] public string ImageSource { get; set; }
		[Browsable(false)] public string ImageExtension { get; set; }
	}
```
## main form properties
```csharp
	private WebBrowser browser = new WebBrowser(); // add a web browser control

	private string defaultLocation = string.Empty;
	private string linkName = string.Empty;
```

## on web browser control's navigating event
```csharp
	private void OnPreparingNavigation(object sender, WebBrowserNavigatingEventArgs e)
	{
	    new Thread(() => 
	    {
		if (this.InvokeRequired)
		{
		    this.Invoke(new Action(() => { this.Text = string.Format("{0} | Navigating...", this.appName); }));
		}
	    }).Start();
	}
```

## on web browser control's document completion event
```csharp
	private void OnVirtualBrowserDocsComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
	{
	    try
	    {
		if (this.browser.ReadyState == WebBrowserReadyState.Complete)
		{
		    this.DoLoadLinkResources();
		}
	    }
	    catch { }
	}
```

## on download link sources
```csharp
	private void DoLoadLinkResources()
        {
            try
            {
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

                this.linkSOurceList = tmpList.Distinct().ToList();
            }
            catch { }
        }
```

## get all img tag
```csharp
	private void GetImages()
	{
		var counter = 1;
		var tmpList = new List<link_sources>();

		foreach (HtmlElement item in this.browser.Document.GetElementsByTagName("IMG"))
		{
			var uri = new Uri(item.GetAttribute("src"));
			if (!string.IsNullOrEmpty(Path.GetExtension(uri.AbsolutePath)))
			{
				var src = item.GetAttribute("src");
				tmpList.Add(new link_sources() { No = counter++, ImageSource = src, ImageExtension = src.Split('.').Last().ToLower() });
			}
		}
	}
```

## download each file
```csharp
	private void Download(List<link_sources> src_list)
	{
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
	}
```

## check file dir
```csharp
	private void check_folder(string file)
	{
		var dir = Path.GetDirectoryName(file);
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
	}

```
## default download location 
```csharp
	c://../desktop/img_grabber_downloads
```
