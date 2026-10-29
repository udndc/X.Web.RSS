using System;
using System.Xml.Serialization;

namespace X.Web.RSS.Structure;

/// <summary>
/// Specifies a GIF, JPEG or PNG image that can be displayed with the channel.
/// </summary>
public record RssImage
{
    private int _height = 31;
    private int _width = 88;

    /// <summary>
    ///   Gets or sets contains text that is included in the TITLE attribute of the link 
    ///   formed around the image in the HTML rendering.
    /// </summary>
    [XmlElement("description")]
    public string Description { get; set; } = "";

    public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);

    /// <summary>
    ///   Gets or sets optional elements include 'width', numbers, indicating the width of the image in pixels.
    /// </summary>
    [XmlElement("width")]
    public int Width
    {
        get => _width;
        set
        {
            if (_width > 144)
            {
                throw new ArgumentException("Width must be less than 144: {_width}", nameof(Width));
            }

            _width = value;
        }
    }

    /// <summary>
    ///   Gets or sets optional elements include 'height', numbers, indicating the height of the image in pixels.
    /// </summary>
    [XmlElement("height")]
    public int Height
    {
        get => _height;
        set
        {
            if (_height > 400)
            {
                throw new ArgumentException("Width must be less than 400: {_height}", nameof(Height));
            }

            _height = value;
        }
    }

    /// <summary>
    ///   Gets or sets is the URL of the site, when the channel is rendered,
    ///   the image is a link to the site.
    /// </summary>
    [XmlElement("link")]
    public RssUrl Link { get; set; } = new();

    public bool ShouldSerializeLink() => !string.IsNullOrEmpty(Link?.Url);

    /// <summary>
    ///   Gets or sets describes the image, it's used in the ALT attribute of the HTML 'img'
    ///   tag when the channel is rendered in HTML.
    /// </summary>
    [XmlElement("title")]
    public string Title { get; set; } = "";

    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty(Title);

    /// <summary>
    ///   Gets or sets is the URL of a GIF, JPEG or PNG image that represents the channel.
    /// </summary>
    [XmlElement("url")]
    public RssUrl Url { get; set; } = new();
}