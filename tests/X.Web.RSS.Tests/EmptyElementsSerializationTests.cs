using System;
using System.Linq;
using System.Xml.Linq;
using X.Web.RSS.Structure;
using Xunit;

namespace X.Web.RSS.Tests;

public class EmptyElementsSerializationTests
{
    private const string Link = "https://example.com/video/1";

    [Fact]
    public void Serialize_MinimalChannel_OmitsUnsetChannelElements()
    {
        var channel = Serialize(CreateMinimalDocument()).Root!.Element("channel")!;

        var names = channel.Elements().Select(o => o.Name.LocalName).ToArray();

        Assert.Equal(
            ["description", "docs", "generator", "pubDate", "link", "title", "item"],
            names);
    }

    [Fact]
    public void Serialize_MinimalItem_OmitsUnsetItemElements()
    {
        var item = Serialize(CreateMinimalDocument()).Root!.Element("channel")!.Element("item")!;

        var names = item.Elements().Select(o => o.Name.LocalName).ToArray();

        Assert.Equal(["description", "guid", "link", "title", "pubDate"], names);
        Assert.Equal(Link, item.Element("guid")!.Value);
        Assert.Equal(Link, item.Element("link")!.Value);
    }

    [Fact]
    public void Serialize_MinimalDocument_HasNoEmptyElementsOrAttributes()
    {
        var xml = Serialize(CreateMinimalDocument());

        var emptyElements = xml.Descendants()
            .Where(o => !o.HasElements && string.IsNullOrEmpty(o.Value) && !o.HasAttributes)
            .Select(o => o.Name.LocalName)
            .ToArray();

        var emptyAttributes = xml.Descendants()
            .SelectMany(o => o.Attributes())
            .Where(o => !o.IsNamespaceDeclaration && string.IsNullOrEmpty(o.Value))
            .Select(o => o.Name.LocalName)
            .ToArray();

        Assert.Empty(emptyElements);
        Assert.Empty(emptyAttributes);
    }

    [Fact]
    public void Serialize_CategoryWithoutDomain_OmitsDomainAttribute()
    {
        var document = CreateMinimalDocument();
        document.Channel.Items[0].Category = new RssCategory { Text = "news" };

        var category = Serialize(document).Root!.Element("channel")!.Element("item")!.Element("category")!;

        Assert.Equal("news", category.Value);
        Assert.Null(category.Attribute("domain"));
    }

    [Fact]
    public void Serialize_ExplicitTtl_WritesTtl()
    {
        var document = CreateMinimalDocument();
        document.Channel.TTL = 30;

        var ttl = Serialize(document).Root!.Element("channel")!.Element("ttl");

        Assert.Equal("30", ttl?.Value);
    }

    private static XDocument Serialize(RssDocument document)
    {
        var xml = new RssDocumentSerializer().Serialize(document);

        return XDocument.Parse(xml);
    }

    private static RssDocument CreateMinimalDocument()
    {
        var document = new RssDocument
        {
            Channel = new RssChannel
            {
                PubDate = new DateTime(2025, 1, 2, 3, 4, 5),
                Title = "Channel title",
                Description = "Channel description",
                Link = new RssUrl("https://example.com/rss")
            }
        };

        document.Channel.Items.Add(new RssItem
        {
            Title = "Item title",
            Description = "Item description",
            Guid = new RssGuid { Value = Link },
            Link = new RssUrl(Link),
            PubDate = new DateTime(2025, 1, 1)
        });

        return document;
    }
}
