namespace FsFlow.Tests

open System.Threading.Tasks
open FsFlow
open FsFlow.Capabilities.Console
open FsFlow.Capabilities.FileSystem
open FsFlow.Capabilities.Http
open FsFlow.Capabilities.Process
open Swensen.Unquote
open Xunit

type UnifiedCaps =
    {
        Console: IConsole
        FS: IFileSystem
        Http: IHttp
        Process: IProcess
    }
    interface IConsole with
        member this.ReadLine() = this.Console.ReadLine()
        member this.WriteLine(message) = this.Console.WriteLine(message)
    interface IFileSystem with
        member this.ReadAllText(path) = this.FS.ReadAllText(path)
        member this.WriteAllText(path, contents) = this.FS.WriteAllText(path, contents)
        member this.Exists(path) = this.FS.Exists(path)
    interface IHttp with
        member this.GetString(url) = this.Http.GetString(url)
    interface IProcess with
        member this.Execute(fileName, arguments) = this.Process.Execute(fileName, arguments)

module CapsUnifiedTests =
    [<Fact>]
    let ``Console: read and write`` () =
        let mutable lastMsg = ""
        let caps = { 
            Console = 
                { new IConsole with 
                    member _.ReadLine() = "input"
                    member _.WriteLine(m) = lastMsg <- m }
            FS = Unchecked.defaultof<_>; Http = Unchecked.defaultof<_>; Process = Unchecked.defaultof<_>
        }
        
        let workflow = flow {
            let! input = Console.readLine
            do! Console.writeLine input
            return input
        }
        
        test <@ Flow.runSync caps workflow = Exit.Success "input" @>
        test <@ lastMsg = "input" @>

    [<Fact>]
    let ``FileSystem: exists and read`` () =
        let caps = { 
            FS = 
                { new IFileSystem with 
                    member _.Exists(_) = true
                    member _.ReadAllText(_) = "content"
                    member _.WriteAllText(_, _) = () }
            Console = Unchecked.defaultof<_>; Http = Unchecked.defaultof<_>; Process = Unchecked.defaultof<_>
        }
        
        let workflow = flow {
            let! exists = FileSystem.exists "test.txt"
            let! text = FileSystem.readAllText "test.txt"
            return exists, text
        }
        
        test <@ Flow.runSync caps workflow = Exit.Success (true, "content") @>

    [<Fact>]
    let ``Http: getString`` () =
        let caps = { 
            Http = { new IHttp with member _.GetString(_) = Task.FromResult "html" }
            Console = Unchecked.defaultof<_>; FS = Unchecked.defaultof<_>; Process = Unchecked.defaultof<_>
        }
        
        test <@ Flow.runSync caps (Http.getString "http://example.com") = Exit.Success "html" @>

    [<Fact>]
    let ``Process: execute`` () =
        let caps = { 
            Process = 
                { new IProcess with 
                    member _.Execute(_, _) = Task.FromResult { ExitCode = 0; StdOut = "out"; StdErr = "" } }
            Console = Unchecked.defaultof<_>; FS = Unchecked.defaultof<_>; Http = Unchecked.defaultof<_>
        }
        
        test <@ Flow.runSync caps (Process.execute "echo" "hi") = Exit.Success { ExitCode = 0; StdOut = "out"; StdErr = "" } @>
