open System

[<EntryPoint>]
let main argv =
    Console.ForegroundColor <- ConsoleColor.White
    printfn "Word Games"
    Console.WriteLine("Please input what you like to do: (Play, Help, Quit)")

    let mutable input = String.Empty

    while not (input.ToUpper() = "QUIT") do
        input <- Console.ReadLine()
        let inputMode = MainMenu.getInputMode(input)
        if not (inputMode = MainMenu.InputMode.Quit) then
            MainMenu.processInput(inputMode)
        Console.Write("\r\n")


    Console.WriteLine("Press anything to continue...")
    Console.ReadKey() |> ignore

    0 // return an integer exit code
