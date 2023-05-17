﻿namespace PdfLibCore;

public readonly struct PdfPageLocation
{
    public float X { get; }
    public float Y { get; }
    public float Zoom { get; }

    public static PdfPageLocation Unknown => new(float.NaN, float.NaN, float.NaN);
    
    public PdfPageLocation(float x, float y, float zoom)
    {
        X = x;
        Y = y;
        Zoom = zoom;
    }

    public override string ToString() =>
        this;

    public static implicit operator string(PdfPageLocation location) =>
        $"(x: {location.X}, y: {location.Y}, zoom: {location.Zoom})";
}