namespace CounterSample;

sealed class MainLayout : PureComponent, IPageLayout
{
    public string ContainerDomElementId => "app";

    public ComponentRenderInfo RenderInfo { get; set; }

    protected override Element render()
    {
        return new html
        {
            Lang("en"),
            DirLtr,
            FontSize13,
            FontFamily("'Roboto', sans-serif"),

            new head
            {
                new meta { charset = "utf-8" },
                new meta { name    = "viewport", content = "width=device-width, initial-scale=1" },
                new title { "Counter Sample" },

                new link
                {
                    rel         = "stylesheet",
                    type        = "text/css",
                    href        = IndexCssFilePath,
                    crossOrigin = "anonymous"
                },

                new style
                {
                    """

                    * {
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }

                    """
                },

                new link { href = "https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap", rel = "stylesheet" }
            },
            new body
            {
                new div(Id(ContainerDomElementId), SizeFull)
            }
        };
    }

    // After page first rendered in client then connect with react system in background.
    // So user first iteraction time will be minimize.
    public string InitialScript =>
        $$"""
          import {ReactWithDotNet} from '{{IndexJsFilePath}}';

          ReactWithDotNet.StrictMode = false;

          ReactWithDotNet.RequestHandlerPath = '{{RequestHandlerPath}}';

          ReactWithDotNet.RenderComponentIn({
            idOfContainerHtmlElement: '{{ContainerDomElementId}}',
            renderInfo: {{RenderInfo.ToJsonString()}}
          });
          """;
}