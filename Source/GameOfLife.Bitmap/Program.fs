namespace GameOfLife

open System.Drawing
open System.Windows.Forms


module Program =
    
    [<EntryPoint>]
    let main args =
        let window = 
            {   Form = new Form(Width = 500, Height = 500)
                Box = new PictureBox(BackColor = Color.White, Dock = DockStyle.Fill) }
        Window.run window
        0

