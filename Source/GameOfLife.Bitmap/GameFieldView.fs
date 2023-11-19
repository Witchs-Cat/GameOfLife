namespace GameOfLife

open System
open System.Drawing
open System.Windows.Forms

module GameFieldView =  
    let brush = new SolidBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00))
    let edgeLength = 10

    let drawCell (graphics : Graphics) (positionX, positionY) = 
        graphics.FillRectangle(brush, positionX, positionY, edgeLength, edgeLength)

    let drawField graphics width height population =
        population
        |> List.map (fun (x, y) -> (x * edgeLength, y * edgeLength)) 
        |> List.filter (fun (x, y)-> x < width && y < height)
        |> List.iter (drawCell graphics)

    let draw (population: Population) (width:int) (height:int) : Bitmap= 
        let image = new Bitmap(width, height)
        let graphics = Graphics.FromImage image
        drawField graphics width height population
        graphics.Dispose()
        image
