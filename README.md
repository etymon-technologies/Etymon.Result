# Etymon.Result

Etymon.Result is a lightweight Result type library for handling success and failure outcomes in C#.

## Features

- Seamless success and failure handling
- Implicit and explicit conversions for cleaner code

## Installation

Use the following command to install the package:

```bash
dotnet add package Etymon.Result
```

## How to Use

**Step 1:**

In service interface change from:

`Task<Model> GetItem(int id);` to `Task<Result<Model>> GetItem(int id)`

**Step 2:**

Implement service

```

public async Task<Result<Model>> GetItem(int id)
{
var data = await \_db.Items.Where(i => i.Id == id).FirstOrDefaultAsync();

    if(data == null)
    {
        return new Error("DataNotFound", "Item your are looking for is not available.");
    }
    return data;

}

```

**Please note that function name has Result but you need to return data only or you can use Result.Success(data)**

**Step 3**
Here is sample controller code:

```

[Route("get-all-items")]
[HttpGet]
public async Task<IActionResult> GetItemsById(int id)
{
    var result = await \_service.GetItems(id);
    if (!result.IsSuccess)
        return NotFound(result);
    return Ok(result );
}

```

**Step 4**

Here is sample output when success:

```

{
    "data": [
        {
            "Id": 1,
            "ItemName": "some item",
            "IsActive": false
        }
    ],
    "error": null,
    "isSuccess": true,
    "isFailure": false
}

```

Here is sample output when error:

```

{
    "data": null,
    "error": {
        "code": "DataNotFound",
        "message": "Item your are looking for is not available."
    },
    "isSuccess": false,
    "isFailure": true
}

```



Additional: _In controller we can have, something like this too:_

```

if (!result.IsSuccess){
    if(result.Error.Code == "DataNotFound")
    {
        return NotFound(result);
    }
    else
    {
        return BadRequest(result);
    }
}

```

Can do `return BadRequest(result.Error.Message);` but then it will not have output as Step 4.

## Updates in 1.1.1

Till Version `1.0.5` we can not have just Result as return type. i.e. if we want to have void as return value then it was not available.

Now it is changed from `1.1.2`

```
public async Task<Result> SaveItem(Item model)
{
    using (var trans = await _db.Database.BeginTransactionAsync())
    {
        try
        {
            var objSave = await _db.Item.Where(x => x.ItemId == model.ItemId).FirstOrDefaultAsync();

            if (objSave == null)
            {
                objSave = new Item();
            }

            objSave.Title = model.Title;

            if (objSave.ItemId > 0)
                _db.Entry(objSave).State = EntityState.Modified;
            else
                _db.Entry(objSave).State = EntityState.Added;

            await _db.SaveChangesAsync();
            await trans.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync();
            return Result.Failure(new Error("UnrecognisedError", "something went wrong!");
        }
    }
}
```
