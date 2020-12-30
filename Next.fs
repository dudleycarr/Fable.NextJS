namespace rec NextJS

open System
open Fable.Core
open Fable.Core.JsInterop
open Node
open Feliz

type IncomingMessage = Http.IncomingMessage
type ServerResponse = Http.ServerResponse

[<AllowNullLiteral>]
type Env =
    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: key:string -> string with get, set

[<AllowNullLiteral>]
type NextApiRequest =
    inherit IncomingMessage
    abstract query: NextApiRequestQuery
    abstract cookies: NextApiRequestCookies
    abstract body: obj option
    abstract env: Env


[<AllowNullLiteral>]
type NextApiResponse =
    inherit ServerResponse
    abstract json: obj -> unit
    abstract status: int -> NextApiResponse
    abstract redirect: string -> NextApiResponse
    abstract redirect: (int * string) -> NextApiResponse

[<AllowNullLiteral>]
type NextApiRequestQuery =
    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: key:string -> U2<string, ResizeArray<string>> with get, set

[<AllowNullLiteral>]
type NextApiRequestCookies =
    [<Emit "$0[$1]{{=$2}}">]
    abstract Item: key:string -> string with get, set

type INextLinkProperty =
    interface
    end

type INextImageProperty =
    interface
    end

type INextHeadProperty =
    interface
    end

module Attr =
    let inline mkNextLinkAttr (key: string) (value: obj): INextLinkProperty = unbox (key, value)
    let inline mkNextImageAttr (key: string) (value: obj): INextImageProperty = unbox (key, value)
    let inline mkNextHeadAttr (key: string) (value: obj): INextHeadProperty = unbox (key, value)

[<Erase>]
type Link =
    static member inline href(value: obj) = Attr.mkNextLinkAttr "href" value
    static member inline asPath(value: string) = Attr.mkNextLinkAttr "as" value
    static member inline passHref(value: bool) = Attr.mkNextLinkAttr "passHref" value
    static member inline prefetch(value: bool) = Attr.mkNextLinkAttr "prefetch" value
    static member inline replace(value: bool) = Attr.mkNextLinkAttr "replace" value
    static member inline scroll(value: bool) = Attr.mkNextLinkAttr "scroll" value
    static member inline shallow(value: bool) = Attr.mkNextLinkAttr "shallow" value

    static member inline text(value: string): INextLinkProperty = unbox (prop.text value)
    static member inline child(value: Fable.React.ReactElement): INextLinkProperty = unbox (prop.children value)

[<Erase>]
[<StringEnum>]
type ImageLayouts =
    | Fixed
    | Instrinsic
    | Responsive
    | Fill

[<Erase>]
type Image =
    static member inline src(value: obj) = Attr.mkNextImageAttr "src" value
    static member inline width(value: int) = Attr.mkNextImageAttr "width" value
    static member inline height(value: int) = Attr.mkNextImageAttr "height" value
    static member inline layout(value: ImageLayouts) = Attr.mkNextImageAttr "layout" value
    static member inline sizes(value: string) = Attr.mkNextImageAttr "sizes" value
    static member inline quality(value: int) = Attr.mkNextImageAttr "quality" value
    static member inline priority(value: bool) = Attr.mkNextImageAttr "priority" value

[<Erase>]
type Head =
    static member inline children(elements: ReactElement list) =
        Attr.mkNextHeadAttr "children" (prop.children elements)

type RouteOptions = { shallow: bool }

[<Erase>]
type RouteEventListener = string -> RouteOptions -> unit

// TODO: Model the error obj
[<Erase>]
type RouteErrorEventListener = Node.Base.Error -> string -> RouteOptions -> unit

[<Erase>]
[<StringEnum>]
type RouterEvent =
    | RouteChangeStart
    | RouteChangeComplete
    | BeforeHistoryChange
    | HashChangeStart
    | HashChangeComplete

[<Erase>]
[<StringEnum>]
type RouterErrorEvent = | RouteChangeError

[<Erase>]
type RouterEventEmitter =
    abstract on: event:RouterEvent * listener:RouteEventListener -> unit
    abstract on: event:RouterErrorEvent * listener:RouteErrorEventListener -> unit
    abstract off: event:RouterEvent * listener:RouteEventListener -> unit
    abstract off: event:RouterErrorEvent * listener:RouteErrorEventListener -> unit

[<Erase>]
type Router =
    abstract pathname: string
    abstract query: obj
    abstract asPath: string
    abstract isFallback: bool
    abstract locale: string
    abstract locales: string list
    abstract defaultLocale: string

    abstract push: url:string -> unit
    abstract push: url:Url.URL -> unit
    abstract push: url:string * asPath:string -> unit
    abstract push: url:Url.URL * asPath:string -> unit
    abstract push: url:string * asPath:string * options:RouteOptions -> unit
    abstract push: url:Url.URL * asPath:string * options:RouteOptions -> unit

    abstract replace: url:string -> unit
    abstract replace: url:Url.URL -> unit
    abstract replace: url:string * asPath:string -> unit
    abstract replace: url:Url.URL * asPath:string -> unit
    abstract replace: url:string * asPath:string * options:RouteOptions -> unit
    abstract replace: url:Url.URL * asPath:string * options:RouteOptions -> unit

    abstract prefetch: url:string -> unit
    abstract prefetch: url:Url.URL -> unit
    abstract prefetch: url:string * asPath:string -> unit
    abstract prefetch: url:Url.URL * asPath:string -> unit

    abstract beforePopState: (string -> option<string> -> RouteOptions -> bool) -> unit
    abstract back: _ -> unit
    abstract reload: _ -> unit
    abstract events: RouterEventEmitter

[<Erase>]
type Next =
    static member inline Link(properties: INextLinkProperty list) =
        Interop.reactApi.createElement (import "default" "next/link", createObj !!properties)

    static member inline Image(properties: INextImageProperty list) =
        Interop.reactApi.createElement (import "default" "next/image", createObj !!properties)

    static member inline Head(properties: INextHeadProperty) =
        Interop.reactApi.createElement (import "default" "next/head", createObj !!properties)

    static member inline useRouter(): Router = import "useRouter" "next/router"
