using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace JoyModels.API.Setups.DatabaseSeed.Helpers;

public static class ImageGenerators
{
    public static void DrawWireframeCube(IImageProcessingContext ctx, float cx, float cy, float cubeSize,
        Color lineColor, Color accentColor, int rotation)
    {
        var halfSize = cubeSize / 2;
        var offset = cubeSize * 0.3f;
        var rotOffset = (rotation % 10) * 5f;

        var frontTopLeft = new PointF(cx - halfSize + rotOffset, cy - halfSize);
        var frontTopRight = new PointF(cx + halfSize + rotOffset, cy - halfSize);
        var frontBottomLeft = new PointF(cx - halfSize, cy + halfSize * 0.6f);
        var frontBottomRight = new PointF(cx + halfSize, cy + halfSize * 0.6f);

        var backTopLeft = new PointF(cx - halfSize + offset + rotOffset, cy - halfSize - offset);
        var backTopRight = new PointF(cx + halfSize + offset + rotOffset, cy - halfSize - offset);
        var backBottomLeft = new PointF(cx - halfSize + offset, cy + halfSize * 0.6f - offset);
        var backBottomRight = new PointF(cx + halfSize + offset, cy + halfSize * 0.6f - offset);

        ctx.DrawLine(lineColor, 2f, frontTopLeft, frontTopRight);
        ctx.DrawLine(lineColor, 2f, frontTopRight, frontBottomRight);
        ctx.DrawLine(lineColor, 2f, frontBottomRight, frontBottomLeft);
        ctx.DrawLine(lineColor, 2f, frontBottomLeft, frontTopLeft);

        ctx.DrawLine(lineColor, 1.5f, backTopLeft, backTopRight);
        ctx.DrawLine(lineColor, 1.5f, backTopRight, backBottomRight);
        ctx.DrawLine(lineColor, 1.5f, backBottomRight, backBottomLeft);
        ctx.DrawLine(lineColor, 1.5f, backBottomLeft, backTopLeft);

        ctx.DrawLine(accentColor, 2f, frontTopLeft, backTopLeft);
        ctx.DrawLine(accentColor, 2f, frontTopRight, backTopRight);
        ctx.DrawLine(lineColor, 1.5f, frontBottomLeft, backBottomLeft);
        ctx.DrawLine(lineColor, 1.5f, frontBottomRight, backBottomRight);
    }

    public static void DrawWireframeSphere(IImageProcessingContext ctx, float cx, float cy, float radius,
        Color lineColor, Color accentColor)
    {
        ctx.Draw(accentColor, 2.5f, new EllipsePolygon(cx, cy, radius, radius));
        ctx.Draw(lineColor, 1.5f, new EllipsePolygon(cx, cy, radius, radius * 0.4f));
        ctx.Draw(lineColor, 1.5f, new EllipsePolygon(cx, cy, radius * 0.4f, radius));

        for (var i = 1; i < 4; i++)
        {
            var r = radius * (i / 4f);
            ctx.Draw(ColorUtilities.DarkenColor(lineColor, 0.2f * i), 1f, new EllipsePolygon(cx, cy, r, r * 0.4f));
        }
    }

    public static void DrawWireframePyramid(IImageProcessingContext ctx, float cx, float cy, float pyramidSize,
        Color lineColor, Color accentColor)
    {
        var halfBase = pyramidSize / 2;
        var height = pyramidSize * 0.8f;

        var apex = new PointF(cx, cy - height);
        var baseLeft = new PointF(cx - halfBase, cy);
        var baseRight = new PointF(cx + halfBase, cy);
        var baseBack = new PointF(cx, cy - halfBase * 0.5f);

        ctx.DrawLine(accentColor, 2.5f, apex, baseLeft);
        ctx.DrawLine(accentColor, 2.5f, apex, baseRight);
        ctx.DrawLine(lineColor, 2f, apex, baseBack);

        ctx.DrawLine(lineColor, 2f, baseLeft, baseRight);
        ctx.DrawLine(lineColor, 1.5f, baseRight, baseBack);
        ctx.DrawLine(lineColor, 1.5f, baseBack, baseLeft);
    }

    public static void DrawWireframeTorus(IImageProcessingContext ctx, float cx, float cy, float majorRadius,
        float minorRadius, Color lineColor, Color accentColor)
    {
        ctx.Draw(accentColor, 2.5f,
            new EllipsePolygon(cx, cy, majorRadius + minorRadius, (majorRadius + minorRadius) * 0.4f));
        ctx.Draw(lineColor, 2f,
            new EllipsePolygon(cx, cy, majorRadius - minorRadius, (majorRadius - minorRadius) * 0.4f));

        for (var i = 0; i < 8; i++)
        {
            var angle = i * MathF.PI / 4;
            var x = cx + majorRadius * MathF.Cos(angle);
            var y = cy + majorRadius * 0.4f * MathF.Sin(angle);
            ctx.Draw(lineColor, 1f, new EllipsePolygon(x, y, minorRadius * 0.3f, minorRadius));
        }
    }

    public static void DrawGameController(IImageProcessingContext ctx, float cx, float cy, float controllerSize,
        Color primaryColor, Color accentColor)
    {
        var bodyWidth = controllerSize * 1.6f;
        var bodyHeight = controllerSize * 0.8f;

        ctx.Fill(primaryColor, new EllipsePolygon(cx - bodyWidth * 0.3f, cy, bodyHeight * 0.5f, bodyHeight * 0.5f));
        ctx.Fill(primaryColor, new EllipsePolygon(cx + bodyWidth * 0.3f, cy, bodyHeight * 0.5f, bodyHeight * 0.5f));
        ctx.Fill(primaryColor,
            new RectangularPolygon(cx - bodyWidth * 0.3f, cy - bodyHeight * 0.25f, bodyWidth * 0.6f,
                bodyHeight * 0.5f));

        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f, cy - 5, 12, 12));
        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f - 15, cy + 5, 8, 8));
        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f + 15, cy + 5, 8, 8));
        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f, cy + 15, 8, 8));

        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f - 12, cy, 10, 10));
        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f + 12, cy, 10, 10));
        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f, cy - 12, 10, 10));
        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f, cy + 12, 10, 10));
    }

    public static void DrawCodeBrackets(IImageProcessingContext ctx, float cx, float cy, float bracketSize,
        Color primaryColor, Color accentColor)
    {
        var thickness = 8f;

        ctx.DrawLine(primaryColor, thickness, new PointF(cx - bracketSize * 0.4f, cy - bracketSize * 0.5f),
            new PointF(cx - bracketSize * 0.6f, cy - bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx - bracketSize * 0.6f, cy - bracketSize * 0.5f),
            new PointF(cx - bracketSize * 0.6f, cy + bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx - bracketSize * 0.6f, cy + bracketSize * 0.5f),
            new PointF(cx - bracketSize * 0.4f, cy + bracketSize * 0.5f));

        ctx.DrawLine(primaryColor, thickness, new PointF(cx + bracketSize * 0.4f, cy - bracketSize * 0.5f),
            new PointF(cx + bracketSize * 0.6f, cy - bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx + bracketSize * 0.6f, cy - bracketSize * 0.5f),
            new PointF(cx + bracketSize * 0.6f, cy + bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx + bracketSize * 0.6f, cy + bracketSize * 0.5f),
            new PointF(cx + bracketSize * 0.4f, cy + bracketSize * 0.5f));

        ctx.DrawLine(accentColor, thickness * 0.8f, new PointF(cx - bracketSize * 0.15f, cy - bracketSize * 0.3f),
            new PointF(cx + bracketSize * 0.15f, cy + bracketSize * 0.3f));
    }

    public static void DrawPixelHeart(IImageProcessingContext ctx, float cx, float cy, float heartSize,
        Color primaryColor, Color accentColor)
    {
        var pixelSize = heartSize / 6f;

        int[,] heartPattern =
        {
            { 0, 1, 1, 0, 0, 1, 1, 0 },
            { 1, 2, 2, 1, 1, 2, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 1 },
            { 0, 1, 2, 2, 2, 2, 1, 0 },
            { 0, 0, 1, 2, 2, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 }
        };

        var startX = cx - (heartPattern.GetLength(1) * pixelSize) / 2;
        var startY = cy - (heartPattern.GetLength(0) * pixelSize) / 2;

        for (var row = 0; row < heartPattern.GetLength(0); row++)
        {
            for (var col = 0; col < heartPattern.GetLength(1); col++)
            {
                if (heartPattern[row, col] == 0) continue;

                var color = heartPattern[row, col] == 1 ? primaryColor : accentColor;
                var x = startX + col * pixelSize;
                var y = startY + row * pixelSize;
                ctx.Fill(color, new RectangularPolygon(x, y, pixelSize - 1, pixelSize - 1));
            }
        }
    }

    public static void DrawLightbulb(IImageProcessingContext ctx, float cx, float cy, float bulbSize,
        Color primaryColor, Color accentColor)
    {
        ctx.Fill(accentColor, new EllipsePolygon(cx, cy - bulbSize * 0.15f, bulbSize * 0.4f, bulbSize * 0.45f));

        ctx.Fill(primaryColor,
            new RectangularPolygon(cx - bulbSize * 0.15f, cy + bulbSize * 0.25f, bulbSize * 0.3f, bulbSize * 0.25f));

        for (var i = 0; i < 3; i++)
        {
            var y = cy + bulbSize * 0.3f + i * 8;
            ctx.DrawLine(ColorUtilities.DarkenColor(primaryColor, 0.2f), 2f, new PointF(cx - bulbSize * 0.12f, y),
                new PointF(cx + bulbSize * 0.12f, y));
        }

        ctx.DrawLine(accentColor, 3f, new PointF(cx - bulbSize * 0.6f, cy - bulbSize * 0.15f),
            new PointF(cx - bulbSize * 0.45f, cy - bulbSize * 0.15f));
        ctx.DrawLine(accentColor, 3f, new PointF(cx + bulbSize * 0.6f, cy - bulbSize * 0.15f),
            new PointF(cx + bulbSize * 0.45f, cy - bulbSize * 0.15f));
        ctx.DrawLine(accentColor, 3f, new PointF(cx, cy - bulbSize * 0.7f),
            new PointF(cx, cy - bulbSize * 0.55f));
    }

    public static void DrawPencil(IImageProcessingContext ctx, float cx, float cy, float pencilSize,
        Color primaryColor, Color accentColor)
    {
        var angle = -45f * MathF.PI / 180f;
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        var bodyLength = pencilSize * 0.8f;
        var bodyWidth = pencilSize * 0.15f;

        var bodyPoints = new PointF[]
        {
            new(cx - bodyLength * 0.5f * cos - bodyWidth * 0.5f * sin,
                cy - bodyLength * 0.5f * sin + bodyWidth * 0.5f * cos),
            new(cx - bodyLength * 0.5f * cos + bodyWidth * 0.5f * sin,
                cy - bodyLength * 0.5f * sin - bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.3f * cos + bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin - bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.3f * cos - bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin + bodyWidth * 0.5f * cos)
        };
        ctx.FillPolygon(primaryColor, bodyPoints);

        var tipPoints = new PointF[]
        {
            new(cx + bodyLength * 0.3f * cos - bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin + bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.3f * cos + bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin - bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.5f * cos, cy + bodyLength * 0.5f * sin)
        };
        ctx.FillPolygon(accentColor, tipPoints);

        ctx.Fill(ColorUtilities.DarkenColor(primaryColor, 0.3f), new EllipsePolygon(
            cx - bodyLength * 0.5f * cos,
            cy - bodyLength * 0.5f * sin,
            bodyWidth * 0.6f, bodyWidth * 0.4f));
    }
}