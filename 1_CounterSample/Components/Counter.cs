namespace CounterSample;

sealed record CounterState
{
    public int Count { get; init; }
}

sealed class Counter : Component<CounterState>
{
    public int Count { get; init; }

    protected override Task constructor()
    {
        state = new()
        {
            Count = Count
        };

        return Task.CompletedTask;
    }

    protected override Element render()
    {
        Style labelStyle =
        [
            FontSize21,
            FontWeightBold,
            CursorDefault,
            Hover(Color(Gray400)),
            UserSelect(none)
        ];
        
        return new FlexRow(AlignItemsCenter, Gap(8))
        {
            new span(labelStyle, OnClick(OnMinusClicked))
            {
                "-"
            },
            state.Count,
            new span(labelStyle, OnClick(OnPlusClicked))
            {
                "+"
            }
        };
    }

    Task OnMinusClicked(MouseEvent e)
    {
        state = state with { Count = state.Count - 1 };

        return Task.CompletedTask;
    }

    Task OnPlusClicked(MouseEvent e)
    {
        state = state with { Count = state.Count + 1 };

        return Task.CompletedTask;
    }
}