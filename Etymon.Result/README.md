# Etymon.Result

**Etymon.Result** is a lightweight C# library for handling success and failure outcomes in a clean and structured way using the Result pattern.

---

## 🚀 Features

- Unified success and failure model
- Supports both `Result<T>` and `Result` (for `void`)
- Clean functional style: `Match`, `Deconstruct`, implicit conversion
- Supports enum-based error codes using `ResultCode`

---

## 📦 Installation

```bash
dotnet add package Etymon.Result
```

---

## ✅ How to Use

### Step 1: Change your service return type

Change this:

```csharp
Task<Model> GetItem(int id);
```

To:

```csharp
Task<Result<Model>> GetItem(int id);
```

---

### Step 2: Implement the service method

```csharp
public async Task<Result<Model>> GetItem(int id)
{
    var data = await _db.Items.FirstOrDefaultAsync(i => i.Id == id);

    if (data == null)
    {
        return new Error("DataNotFound", "Item you're looking for is not available.");
        // OR (recommended from v1.2+)
        // return Result<Model>.Failure(ResultCode.NotFound, "Item you're looking for is not available.");
    }

    return data; // Implicitly converted to Result<Model>.Success(data)
}
```

> ✅ **Tip:** You can use `Result.Success(data)` explicitly, or just return the `data` directly.

---

### Step 3: Controller usage

```csharp
[Route("get-item")]
[HttpGet]
public async Task<IActionResult> GetItemById(int id)
{
    var result = await _service.GetItem(id);

    if (!result.IsSuccess)
    {
        if (result.Error?.Code == "DataNotFound")
            return NotFound(result);
        else
            return BadRequest(result);
    }

    return Ok(result);
}
```

> ⚠️ If you use `BadRequest(result.Error.Message)` you'll lose the full error structure (see Step 4 for sample outputs).

---

### Step 4: Output Examples

#### ✅ Success response:

```json
{
  "data": {
    "id": 1,
    "itemName": "some item",
    "isActive": false
  },
  "error": null,
  "isSuccess": true
}
```

#### ❌ Failure response:

```json
{
  "data": null,
  "error": {
    "code": "DataNotFound",
    "message": "Item you're looking for is not available."
  },
  "isSuccess": false
}
```

---

## 🔧 Update in v1.1.2 – `Result` without return value (`void`-like)

Prior to `v1.1.2`, `Result<T>` was required. Now, you can return just `Result` for operations that don’t return a value:

```csharp
public async Task<Result> SaveItem(Item model)
{
    using var trans = await _db.Database.BeginTransactionAsync();

    try
    {
        var obj = await _db.Items.FirstOrDefaultAsync(x => x.ItemId == model.ItemId) ?? new Item();

        obj.Title = model.Title;

        _db.Entry(obj).State = obj.ItemId > 0 ? EntityState.Modified : EntityState.Added;

        await _db.SaveChangesAsync();
        await trans.CommitAsync();

        return Result.Success();
    }
    catch (Exception ex)
    {
        await trans.RollbackAsync();
        return Result.Failure(new Error("UnrecognisedError", "Something went wrong!"));
    }
}
```

---
## 🆕 Update in v1.2.0 – Enum-based Error Support

In version `1.2.0`, we've added support for `ResultCode` enum for standardized error handling.

You can now create errors like this:

```csharp
return Result<Model>.Failure(ResultCode.NotFound, "Item not found.");
```

This allows you to use consistent and meaningful codes instead of free-form strings, making controller mapping and API responses cleaner.

---

### 🧱 Built-in `ResultCode` Enum

```csharp
public enum ResultCode
{
    Success,
    NotFound,
    ValidationError,
    Unauthorized,
    Conflict,
    InternalError
}
```

> The `Error` class now supports both string-based and enum-based codes.

## ✨ Extension Package: Etymon.Result.Extensions

For cleaner controller code and automatic HTTP status mapping, install the optional extensions package:

```bash
dotnet add package Etymon.Result.Extensions
```

Then use the `ToActionResult()` helper in your controller:

```csharp
using Etymon.Result.Extensions;

[HttpGet("{id}")]
public async Task<IActionResult> GetItem(int id)
{
    var result = await _service.GetItem(id);
    return result.ToActionResult();
}
```

This maps `ResultCode` to appropriate HTTP responses like `200 OK`, `404 NotFound`, `400 BadRequest`, etc., so you don't have to write repetitive logic.


## 🔄 Coming Soon (Planned Features)

- Result middleware for automatic API response wrapping
- Built-in integration with `FluentValidation`
- Localization support in `Error`

---

## ❤️ Contribute / Feedback

Feel free to open issues or pull requests for enhancements or bug reports.
