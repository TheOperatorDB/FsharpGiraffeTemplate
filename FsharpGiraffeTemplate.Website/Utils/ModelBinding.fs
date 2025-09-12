module FsharpGiraffeTemplate.Website.Utils.ModelBinding

open Microsoft.AspNetCore.Mvc.ModelBinding
open Microsoft.AspNetCore.Mvc.ModelBinding.Binders
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System
open System.Threading.Tasks

type EmptyResizeArrayModelBinder<'T>(elementBinder, loggerFactory) =
    let collectionBinder = CollectionModelBinder<'T>(elementBinder, loggerFactory)

    interface IModelBinder with
        member this.BindModelAsync(bindingContext) =
            ArgumentNullException.ThrowIfNull(bindingContext)

            let containsComplexObject =
                bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName)

            let valueProviderResult =
                bindingContext.ValueProvider.GetValue(bindingContext.ModelName)

            if
                valueProviderResult = ValueProviderResult.None
                && not containsComplexObject
            then
                bindingContext.Result <- ModelBindingResult.Success(ResizeArray<'T>([||]))
                Task.CompletedTask
            else
                collectionBinder.BindModelAsync(bindingContext)

type EmptyResizeArrayModelBinderProvider() =
    interface IModelBinderProvider with
        member this.GetBinder(context) =
            ArgumentNullException.ThrowIfNull(context)

            if
                (context.Metadata.ModelType.IsGenericType
                 && context.Metadata.ModelType.GetGenericTypeDefinition() = typedefof<System.Collections.Generic.List<_>>)
            then
                let elementType = context.Metadata.ElementMetadata.ModelType

                let binderType =
                    typedefof<EmptyResizeArrayModelBinder<_>>
                        .MakeGenericType(elementType)

                let elementBinder = context.CreateBinder(context.Metadata.ElementMetadata)
                let loggerFactory = context.Services.GetRequiredService<ILoggerFactory>()
                Activator.CreateInstance(binderType, elementBinder, loggerFactory) :?> IModelBinder
            else
                null
