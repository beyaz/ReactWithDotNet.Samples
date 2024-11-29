namespace CounterSample;

sealed record PageRouteInfo(string Url, Type page);

static class PageList
{
    public static readonly PageRouteInfo Home = new("/", typeof(HomePage));
}