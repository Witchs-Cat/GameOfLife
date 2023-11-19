namespace GameOfLife

open System

type Cell = Tuple<int, int>
type Population = Cell List

module GameField =
    let neighbours (x, y) = 
        [   for i in x-1..x+1 do
            for j in y-1..y+1 do
            if not (i = x && j = y) then    
                yield (i, j)]
    

    let isAlive (population: Population) (cell : Cell) = 
        population 
        |> List.exists ((=) cell)

    let aliveNeighbours (population: Population) (cell: Cell) =
        neighbours cell
        |> List.filter (isAlive population)

    let survives (population: Population) (cell: Cell) = 
        aliveNeighbours population cell
        |> List.length
        |> fun x -> x >= 2 && x <= 3
    
    let reproducible (population: Population) (cell: Cell) = 
        aliveNeighbours population cell
        |> List.length = 3

    //метод вернет только те мертвые клетки, у которых есть живые соседи
    let allDeadNeighbours (population: Population) = 
        population
        |> List.collect neighbours
        |> Set.ofList |> Set.toList
        |> List.filter (not << isAlive population)
        
    let randomGeneration width heigth =
        let random = Random();
        
        [   for i in 0..width do
            for j in 0..heigth do
            if random.Next(0, 2) = 0 then    
                yield (i, j)]

    let nextGeneration (population: Population) widthLimit heightLimit = 
        List.append
            (population
            |> List.filter(survives population))
            (allDeadNeighbours population
            |> List.filter(reproducible population))
            |> List.filter(fun (x,y) -> x < widthLimit && y < heightLimit)