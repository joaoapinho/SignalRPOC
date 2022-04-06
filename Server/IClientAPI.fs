module Giraffe1.ClientApi

type IClientApi =
    abstract member LoginResponse :bool * string -> System.Threading.Tasks.Task
    abstract member Message :string -> System.Threading.Tasks.Task
    abstract member State :string -> System.Threading.Tasks.Task

