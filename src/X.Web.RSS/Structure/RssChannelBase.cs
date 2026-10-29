using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using JetBrains.Annotations;
using X.Web.RSS.Extensions;
using X.Web.RSS.Enumerators;
using X.Web.RSS.Validators;

namespace X.Web.RSS.Structure;

[PublicAPI]
public abstract record RssChannelBase
{
    private int _ttl;
    private string? _language;

    public RssChannelBase()
    {
        Docs = "http://www.rssboard.org/rss-specification";
        Generator = "X.Web.RSS Generator";
        SkipDays = [];
        SkipHours = [];
        AtomLink = new RssLink
        {
            Rel = Rel.self,
            Type = "application/rss+xml"
        };
        Category = "";
        Cloud = new RssCloud();
        Copyright ="";
        Description = "";
        Image = new RssImage();
        Link = new RssUrl();
        ManagingEditor = "";
        Rating = "";
        TextInput = new RssTextInput();
        Title = "";
        WebMaster = "";
    }

    /// <summary>
    /// Gets or sets &lt;atom:link href = "http://bash.org.ru/rss/" rel = "self" type = "application/rss+xml" /&gt;
    /// </summary>
    [XmlElement("link", Namespace = "http://www.w3.org/2005/Atom")]
    public RssLink AtomLink { get; set; }

    public bool ShouldSerializeAtomLink() => !string.IsNullOrEmpty(AtomLink?.Href);

    /// <summary>
    /// Gets or sets one or more categories that the channel belongs to. 
    /// Follows the same rules as the 'item'-level category element.
    /// </summary>
    [XmlElement("category")]
    public string Category { get; set; }

    public bool ShouldSerializeCategory() => !string.IsNullOrEmpty(Category);

    /// <summary>
    /// Gets or sets processes to register with a cloud to be notified of 
    /// updates to the channel, implementing a lightweight publish-subscribe 
    /// protocol for RSS feeds.
    /// </summary>
    [XmlElement("cloud")]
    public RssCloud Cloud { get; set; }

    public bool ShouldSerializeCloud() => !string.IsNullOrEmpty(Cloud?.Domain);

    /// <summary>
    /// Gets or sets copyright notice for content in the channel.
    /// </summary>
    /// <example>
    /// Copyright 2002, Spartanburg Herald-Journal
    /// </example>
    [XmlElement("copyright")]
    public string Copyright { get; set; }

    public bool ShouldSerializeCopyright() => !string.IsNullOrEmpty(Copyright);

    /// <summary>
    /// Gets or sets phrase or sentence describing the channel.
    /// </summary>
    /// <example>
    /// The latest news from GoUpstate.com, a Spartanburg Herald-Journal Web site.
    /// </example>
    [XmlElement("description")]
    public string Description { get; set; }

    public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);

    /// <summary>
    /// Gets or sets URL that points to the documentation for the format used in the RSS file. 
    /// It's probably a pointer to this page. It's for people who might stumble across 
    /// an RSS file on a Web server 25 years from now and wonder what it is.
    /// </summary>
    /// <example>
    /// http://www.rssboard.org/rss-specification
    /// </example>
    [XmlElement("docs")]
    public string Docs { get; set; }

    public bool ShouldSerializeDocs() => !string.IsNullOrEmpty(Docs);

    /// <summary>
    /// Gets or sets a string indicating the program used to generate the channel.
    /// </summary>
    /// <example>
    /// MightyInHouse Content System v2.3
    /// </example>
    [XmlElement("generator")]
    public string Generator { get; set; }

    public bool ShouldSerializeGenerator() => !string.IsNullOrEmpty(Generator);

    /// <summary>
    /// Gets or sets a GIF, JPEG or PNG image that can be displayed with the channel.
    /// </summary>
    [XmlElement("image")]
    public RssImage Image { get; set; }

    public bool ShouldSerializeImage() => !string.IsNullOrEmpty(Image?.Url?.Url);

    /// <summary>
    /// Gets or sets OPTIONAL the language the channel is written in. This allows aggregators to group all 
    /// Italian language sites, for example, on a single page. A list of allowable 
    /// values for this element, as provided by Netscape, is 
    /// http://www.rssboard.org/rss-language-codes. You may also use values defined 
    /// by the W3C http://www.w3.org/TR/REC-html40/struct/dirlang.html#langcodes.
    /// </summary>
    /// <example>
    /// en-us
    /// </example>
    [XmlElement("language")]
    public string? Language
    {
        get { return _language; }
        set
        {
            new LanguageValidator().Validate(value);
            _language = value;
        }
    }

    [XmlElement("lastBuildDate")]
    public string InternalLastBuildDate
    {
        get => LastBuildDate?.ToRFC822Date() ?? string.Empty;
        set => LastBuildDate = value?.FromRFC822Date();
    }

    public bool ShouldSerializeInternalLastBuildDate() => LastBuildDate.HasValue;

    [XmlElement("pubDate")]
    public string InternalPubDate
    {
        get => PubDate?.ToRFC822Date() ?? string.Empty;
        set => PubDate = value?.FromRFC822Date();
    }

    public bool ShouldSerializeInternalPubDate() => PubDate.HasValue;

    /// <summary>
    /// Gets or sets the last time the content of the channel changed.
    /// </summary>
    /// <example>
    /// Sat, 07 Sep 2002 09:42:31 GMT
    /// </example>
    [XmlIgnore]
    public DateTime? LastBuildDate { get; set; }

    /// <summary>
    /// Gets or sets the URL to the HTML website corresponding to the channel.
    /// </summary>
    /// <example>
    /// http://www.goupstate.com/
    /// </example>
    [XmlElement("link")]
    public RssUrl Link { get; set; }

    public bool ShouldSerializeLink() => !string.IsNullOrEmpty(Link?.Url);

    /// <summary>
    /// Gets or sets email address for person responsible for editorial content.
    /// </summary>
    /// <example>
    /// geo@herald.com (George Matesky)
    /// </example>
    [XmlElement("managingEditor")]
    public string ManagingEditor { get; set; }

    public bool ShouldSerializeManagingEditor() => !string.IsNullOrEmpty(ManagingEditor);

    /// <summary>
    /// Gets or sets the publication date for the content in the channel. 
    /// For example, the New York Times publishes on a daily basis, 
    /// the publication date flips once every 24 hours. That's when 
    /// the pubDate of the channel changes. All date-times in RSS
    /// conform to the Date and Time Specification of RFC 822
    /// http://asg.web.cmu.edu/rfc/rfc822.html, with the exception that
    /// the year may be expressed with two characters or four characters 
    /// (four preferred).
    /// </summary>
    /// <example>
    /// Sat, 07 Sep 2002 00:00:01 GMT
    /// </example>
    [XmlIgnore]
    public DateTime? PubDate { get; set; }

    /// <summary>
    /// Gets or sets the PICS rating for the channel.
    /// </summary>
    [XmlElement("rating")]
    public string Rating { get; set; }

    public bool ShouldSerializeRating() => !string.IsNullOrEmpty(Rating);

    /// <summary>
    /// Gets or sets a hint for aggregators telling them which days they can skip. 
    /// This element contains up to seven 'day' sub-elements whose value 
    /// is Monday, Tuesday, Wednesday, Thursday, Friday, Saturday or 
    /// Sunday. Aggregators may not read the channel during days listed 
    /// in the 'skipDays' element.
    /// </summary>
    [XmlArrayItem("day")]
    [XmlArray("skipDays")]
    public List<Day> SkipDays { get; set; }

    [XmlIgnore]
    public bool SkipDaysSpecified => SkipDays.Count > 0;

    /// <summary>
    /// Gets or sets a hint for aggregators telling them which hours they can skip.
    /// This element contains up to 24 'hour' sub-elements whose value is
    /// a number between 0 and 23, representing a time in GMT, when aggregators, 
    /// if they support the feature, may not read the channel on hours listed 
    /// in the 'skipHours' element. The hour beginning at midnight is hour zero.
    /// </summary>
    [XmlArrayItem("hour")]
    [XmlArray("skipHours")]
    public List<Hour> SkipHours { get; set; }

    [XmlIgnore]
    public bool SkipHoursSpecified => SkipHours.Count > 0;

    /// <summary>
    /// Gets or sets ttl stands for time to live. It's a number of minutes that indicates how 
    /// long a channel can be cached before refreshing from the source.
    /// </summary>
    [XmlElement("ttl")]
    public int TTL
    {
        get => _ttl;
        set
        {
            new TTLValidator().Validate(value);

            _ttl = value;
        }
    }

    public bool ShouldSerializeTTL() => TTL > 0;

    /// <summary>
    /// Gets or sets a text input box that can be displayed with the channel. OBSOLETE.
    /// </summary>
    [XmlElement("textInput")]
    public RssTextInput TextInput { get; set; }

    public bool ShouldSerializeTextInput() =>
        TextInput != null &&
        (!string.IsNullOrEmpty(TextInput.Title) ||
         !string.IsNullOrEmpty(TextInput.Description) ||
         !string.IsNullOrEmpty(TextInput.Name) ||
         !string.IsNullOrEmpty(TextInput.Link?.Url));

    /// <summary>
    /// Gets or sets the name of the channel. It's how people refer to your service. 
    /// If you have an HTML website that contains the same information as 
    /// your RSS file, the title of your channel should be the same as 
    /// the title of your website.
    /// </summary>
    /// <example>
    /// http://www.goupstate.com/
    /// </example>
    [XmlElement("title")]
    public string Title { get; set; }

    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);

    /// <summary>
    /// Gets or sets email address for person responsible for technical issues relating to channel.
    /// </summary>
    /// <example>
    /// betty@herald.com (Betty Guernsey)
    /// </example>
    [XmlElement("webMaster")]
    public string WebMaster { get; set; }

    public bool ShouldSerializeWebMaster() => !string.IsNullOrEmpty(WebMaster);
}