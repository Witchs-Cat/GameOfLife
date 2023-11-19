namespace GameOfLife

open System
open System.Drawing
open System.Windows.Forms

type Window = 
    {   Form: Form
        Box: PictureBox }

type FPSCounter = 
    {   LastUpdate: DateTimeOffset
        FramesCount: int
        FPS: int}

type Context = 
    {   Window: Window
        FPSCounter: FPSCounter
        Population: Population }

module Window =         
    let getFieldWidth window = 
        window.Form.Width / GameFieldView.edgeLength

    let getFieldHeight window =
        window.Form.Height / GameFieldView.edgeLength

    let swapImage window image = 
        let oldImage = window.Box.Image
        window.Box.Image <- image
 
        if not(isNull oldImage) then
            oldImage.Dispose ()
    
    let calculateFps fpsCounter =
        let now = DateTimeOffset.Now
        let span = now - fpsCounter.LastUpdate
        
        if  span > TimeSpan.FromSeconds 1 then
            {   LastUpdate = now
                FramesCount = 0;
                FPS = ((fpsCounter.FramesCount  + 1)/ span.Seconds) }
        else 
            {   LastUpdate = fpsCounter.LastUpdate
                FramesCount = fpsCounter.FramesCount + 1
                FPS = fpsCounter.FPS }

    let update context =
        let population = context.Population
        let fpsCounter = context.FPSCounter
        let window = context.Window
        
        let width = window.Form.Width 
        let height = window.Form.Height

        let newPopulatioin = GameField.nextGeneration population (getFieldWidth window) (getFieldHeight window)

        GameFieldView.draw newPopulatioin width height
        |> swapImage window

        let newFPSCounter = calculateFps fpsCounter
        window.Form.Text <- 
            "FPS: " + newFPSCounter.FPS.ToString() 
            + " W:" + width.ToString() 
            + " H:" + height.ToString()

        {   Window = context.Window
            FPSCounter = newFPSCounter
            Population = newPopulatioin }

    let rec mainLoop context =
        Application.DoEvents ()
        
        let updatedContex = update context

        if Application.OpenForms.Count > 0 then
            mainLoop updatedContex

    let run (window: Window) =  
        let init args = 
            let population = GameField.randomGeneration (getFieldWidth window) (getFieldWidth window)
            let fpsCounter = 
                {   LastUpdate = DateTimeOffset.Now
                    FramesCount = 0
                    FPS = 0 }
            let context = 
                {   Window = window
                    FPSCounter = fpsCounter
                    Population = population }

            mainLoop context

        window.Form.Shown.Add(init)
        window.Form.Controls.Add(window.Box)
        Application.Run (window.Form)