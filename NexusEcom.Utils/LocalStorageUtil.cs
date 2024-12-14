using Microsoft.JSInterop;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LocalStorageUtil : IIWLocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageUtil(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        var jsonValue = System.Text.Json.JsonSerializer.Serialize(value);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, jsonValue);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var jsonValue = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        return jsonValue == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(jsonValue);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task RemoveAllItemsAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }

    void IIWProtectedBrowserStorageService.SetItem<T>(string key, T value)
    {
        throw new NotImplementedException();
    }

    T? IIWProtectedBrowserStorageService.GetItem<T>(string key) where T : default
    {
        throw new NotImplementedException();
    }

    void IIWProtectedBrowserStorageService.RemoveItem(string key)
    {
        throw new NotImplementedException();
    }

    void IIWProtectedBrowserStorageService.RemoveAllItems()
    {
        throw new NotImplementedException();
    }
}

public interface ILocalStorageService
{
    Task SetItemAsync<T>(string key, T value);
    Task<T?> GetItemAsync<T>(string key);
    Task RemoveItemAsync(string key);
}
