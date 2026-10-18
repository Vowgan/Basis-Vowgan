using System;
using UnityEngine;

public class CameraModeEntry
{

    public string title { get; private set; }
    public string label { get; private set; }
    public Sprite icon { get; private set; }
    public int order { get; private set; }
    public bool selected { get; private set; }

    public event Action onRun;
    public event Action onSelect;
    public event Action onDeselect;


    public CameraModeEntry(string title, Sprite icon, int order, Action onRun)
    {
        this.title = title;
        label = title;

        this.icon = icon;
        this.order = order;
        this.onRun = onRun;
    }

    public CameraModeEntry(string title, Sprite icon, int order, bool selected, Action onSelect, Action onDeselect)
    {
        this.title = title;
        label = title;

        this.icon = icon;
        this.order = order;
        this.selected = selected;
        this.onSelect = onSelect;
        this.onDeselect = onDeselect;
    }


    public void Run()
    {
        onRun?.Invoke();
    }

    public void SetSelected(bool value)
    {
        if (selected == value) return;
        selected = value;
        if (value) onSelect?.Invoke();
        else onDeselect?.Invoke();
    }

    public void SetLabel(string newLabel)
    {
        label = newLabel;
    }

}
