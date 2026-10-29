using System;
using System.Xml.Serialization;
using X.Web.RSS.Extensions;

namespace X.Web.RSS.Structure;

/// <summary>
/// A channel may contain any number of 'item's. An item may represent 
///   a "story" -- much like a story in a newspaper or magazine; if so its
///   description is a synopsis of the story, and the link points to the full 
///   story. An item may also be complete in itself, if so, the description 
///   contains the text (entity-encoded HTML is allowed; see examples
///   http://www.rssboard.org/rss-encoding-examples), and the link and title may 
///   be omitted. All elements of an item are optional, however at least one of 
///   title or description must be present.
/// </summary>
public record RssItem
{
    /// <summary>
    ///   Gets or sets email address of the author of the item.
    /// </summary>
    [XmlElement("author")]
    public string Author { get; set; } = "";

    public bool ShouldSerializeAuthor() => !string.IsNullOrEmpty(Author);

    /// <summary>
    ///   Gets or sets includes the item in one or more categories.
    /// </summary>
    [XmlElement("category")]
    public RssCategory Category { get; set; } = new();

    public bool ShouldSerializeCategory() => !string.IsNullOrEmpty(Category?.Text);

    /// <summary>
    ///   Gets or sets URL of a page for comments relating to the item.
    /// </summary>
    [XmlElement("comments")]
    public RssUrl Comments { get; set; } = new();

    public bool ShouldSerializeComments() => !string.IsNullOrEmpty(Comments?.Url);

    /// <summary>
    ///   Gets or sets the item synopsis.
    /// </summary>
    /// <example>
    ///   Some of the most heated chatter at the Venice Film Festival this 
    ///   week was about the way that the arrival of the stars at the Palazzo 
    ///   del Cinema was being staged.
    /// </example>
    [XmlElement("description")]
    public string Description { get; set; } = "";

    public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);

    /// <summary>
    ///   Gets or sets describes a media object that is attached to the item.
    /// </summary>
    [XmlElement("enclosure")]
    public RssEnclosure Enclosure { get; set; } = new();

    public bool ShouldSerializeEnclosure() => !string.IsNullOrEmpty(Enclosure?.Url);

    /// <summary>
    ///   Gets or sets a string that uniquely identifies the item.
    /// </summary>
    [XmlElement("guid")]
    public RssGuid Guid { get; set; } = new();

    public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid?.Value);

    /// <summary>
    ///   Gets or sets the URL of the item.
    /// </summary>
    /// <example>
    ///   http://nytimes.com/2004/12/07FEST.html
    /// </example>
    [XmlElement("link")]
    public RssUrl Link { get; set; } = new();

    public bool ShouldSerializeLink() => !string.IsNullOrEmpty(Link?.Url);

    /// <summary>
    ///   Gets or sets indicates when the item was published.
    /// </summary>
    [XmlIgnore]
    public DateTime? PubDate { get; set; }

    /// <summary>
    ///   Gets or sets the RSS channel that the item came from
    /// </summary>
    [XmlElement("source")]
    public RssSource Source { get; set; } = new();

    public bool ShouldSerializeSource() => !string.IsNullOrEmpty(Source?.Url);

    /// <summary>
    ///   Gets or sets the title of the item.
    /// </summary>
    /// <example>
    ///   Venice Film Festival Tries to Quit Sinking
    /// </example>
    [XmlElement("title")]
    public string Title { get; set; } = "";

    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);

    [XmlElement("pubDate")]
    public string InternalPubDate
    {
        get => PubDate?.ToRFC822Date() ?? string.Empty;
        set => PubDate = value?.FromRFC822Date();
    }

    public bool ShouldSerializeInternalPubDate() => PubDate.HasValue;
}