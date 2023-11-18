using Godot;
using System;
using Terriflux.Programs.Model.Placeables;
using System.Collections.Generic;
using Terriflux.Programs.GameContext;

public partial class Inventory : Node2D, IIventory
{
    // quantities management
    private readonly Dictionary<FlowKind, int> quantitiesProduced = new();    // all quantities produced in one turn
    private readonly Dictionary<FlowKind, int> quantitiesNeeded = new();    // all quantities needed for one turn

    // children
    private readonly Dictionary<FlowKind, Sprite2D> _arrows = new(); // quantities arrows visible for user
    private readonly Dictionary<FlowKind, Label> _quantitiesLabels = new();    // quantities label visible for user

    // static textures
    private static readonly Texture2D _constArrow = GD.Load<Texture2D>(OurPaths.TEXTURES + "const.png");
    private static readonly Texture2D _upArrow = GD.Load<Texture2D>(OurPaths.ICONS + "up.png");
    private static readonly Texture2D _downArrow = GD.Load<Texture2D>(OurPaths.ICONS + "down.png");

    // CONSTRUCT
    private Inventory() { }

    public static Inventory Design()
    {
        return (Inventory)GD.Load<PackedScene>("tblInventaire.tscn")
            .Instantiate();
    }

    public void Add(FlowKind flow, int amount)
    {
        quantitiesProduced[flow] += amount;

        // update hud
        Update(flow);
    }

    public void Remove(FlowKind flow, int amount)
    {
        quantitiesProduced[flow] -= amount;

        // update hud
        Update(flow);
    }

    public bool Contains(FlowKind flow)
    {
        return quantitiesProduced.ContainsKey(flow);
    }

    public bool ContainsEnough(FlowKind flow, int amount)
    {
        return this.Contains(flow) && this.quantitiesProduced[flow] > amount;
    }

    private void Update(FlowKind flow)
    {
        int variation = this.quantitiesProduced[flow] - this.quantitiesNeeded[flow];

        // overproduction?
        if (variation > 0)
        {
            this._arrows[flow].Texture = _upArrow; // arrow to up
            this._quantitiesLabels[flow].Text = "+" + variation;
            this._quantitiesLabels[flow].Modulate = new(0, 10, 0); // green
        }
        // perfect balance between product and necessity?
        else if (variation == 0)
        {
            this._arrows[flow].Texture = _constArrow; // vertical bar
            this._quantitiesLabels[flow].Text = "0";
            this._quantitiesLabels[flow].Modulate = new(255, 255, 255); // black
        }
        // underproduction?
        else
        {
            this._arrows[flow].Texture = _downArrow; //  arrow to down
            this._quantitiesLabels[flow].Text = "-" + variation;
            this._quantitiesLabels[flow].Modulate = new(10, 0, 0); // red
        }
    }



    // GODOT
    public override void _Ready()
    {
        // add references for arrows
        this._arrows.Add(FlowKind.ENERGY, GetNode<Sprite2D>("VarEnergie"));
        this._arrows.Add(FlowKind.RAW_MATERIAL, GetNode<Sprite2D>("VarMatiere"));
        this._arrows.Add(FlowKind.WATER, GetNode<Sprite2D>("VarEau"));
        this._arrows.Add(FlowKind.MANUFACTURED_MERCHANDISE, GetNode<Sprite2D>("VarMarchandise"));
        this._arrows.Add(FlowKind.CEREALS, GetNode<Sprite2D>("VarCereales"));
        this._arrows.Add(FlowKind.BREAD, GetNode<Sprite2D>("VarPain"));

        // add references for quantities
        this._quantitiesLabels.Add(FlowKind.ENERGY, GetNode<Label>("VarNumberEnergie"));
        this._quantitiesLabels.Add(FlowKind.RAW_MATERIAL, GetNode<Label>("VarNumberMatiere"));
        this._quantitiesLabels.Add(FlowKind.WATER, GetNode<Label>("VarNumberEau"));
        this._quantitiesLabels.Add(FlowKind.MANUFACTURED_MERCHANDISE, GetNode<Label>("VarNumberMarchandise"));
        this._quantitiesLabels.Add(FlowKind.CEREALS, GetNode<Label>("VarNumberCereales"));
        this._quantitiesLabels.Add(FlowKind.BREAD, GetNode<Label>("VarNumberPain"));

        // reset arrows 
        foreach (Sprite2D arrow in _arrows.Values)
        {
            arrow.Texture = _constArrow;
        }

        // reset labels
        foreach (Label label in _quantitiesLabels.Values)
        {
            label.Text = "0";
            label.Modulate = new(255, 255, 255); // black
        }
    }

}
