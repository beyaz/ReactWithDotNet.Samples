namespace CounterSample;

sealed class HomePage : PureComponent
{
    protected override Element render()
    {
        return new FlexRowCentered(Padding(50), Border(1, solid, Gray300), SizeFitContent, BorderRadius(8))
        {
            new Counter { Count = 19 }
        };
    }
}