using System.Xml.Serialization;

namespace X.Web.RSS.Structure;

public record RssCategory
{
    public RssCategory()
    {
        Domain = "";
        Text = "";
    }

    [XmlAttribute("domain")]
    public string Domain { get; set; }

    public bool ShouldSerializeDomain() => !string.IsNullOrEmpty(Domain);

    [XmlText]
    public string Text { get; set; }
}