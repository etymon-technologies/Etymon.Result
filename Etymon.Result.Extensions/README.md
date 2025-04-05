# Etymon.Result.Extensions

**Etymon.Result.Extensions** provides helper extensions for working with [Etymon.Result](https://www.nuget.org/packages/Etymon.Result), specifically for ASP.NET Core Web APIs. It simplifies controller code by converting `Result<T>` objects into appropriate HTTP responses using `ToActionResult()`.

---

## 📦 Installation

```bash
dotnet add package Etymon.Result.Extensions
```

> Requires: `.NET 6.0` or higher and reference to `Etymon.Result`.

---

## ✨ Features

- `ToActionResult()` extension for clean and consistent controller responses
- Maps `ResultCode` (e.g. `NotFound`, `ValidationError`) to standard HTTP responses
- Reduces repetitive error handling logic in controller actions

---

## ✅ How to Use

### 1. Add a reference to your service that returns `Result<T>`:

```csharp
public async Task<Result<ItemDto>> GetItem(int id);
```

---

### 2. In your controller:

```csharp
using Etymon.Result.Extensions;

[HttpGet("{id}")]
public async Task<IActionResult> GetItem(int id)
{
    var result = await _itemService.GetItem(id);
    return result.ToActionResult();
}
```

---

### 3. Output Samples

#### ✅ Success (HTTP 200):

```json
{
  "data": {
    "id": 1,
    "name": "Sample Item"
  },
  "error": null,
  "isSuccess": true
}
```

#### ❌ Failure (e.g. HTTP 404):

```json
{
  "data": null,
  "error": {
    "code": "NotFound",
    "message": "Item was not found."
  },
  "isSuccess": false
}
```

---

## 🔧 How It Works

The `ToActionResult()` method maps `ResultCode` to standard HTTP results:

| `ResultCode`       | HTTP Response       |
|--------------------|---------------------|
| `Success`          | `200 OK`            |
| `NotFound`         | `404 Not Found`     |
| `ValidationError`  | `400 Bad Request`   |
| `Unauthorized`     | `403 Forbidden`     |
| `Conflict`         | `409 Conflict`      |
| `InternalError`    | `500 Internal Error`|

---

## 📄 License

MIT

---

## 🤝 Contributing

Feel free to open issues or submit PRs to extend support for non-generic `Result`, middleware integration, or advanced model binding.
