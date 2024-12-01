using System;

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int newId = Config.NextCallId;
        Call newCall = item with { Id = newId };
        DataSource.Calls.Add(newCall);
    }
    public void Delete(int id)
    {
        Call? currentCall = Read(id);
        if (currentCall != null)
            DataSource.Calls.Remove(currentCall);
        else
            throw new Exception($"call with id {id} no exists");

    }

    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
        //return DataSource.Calls.Find(c => c.Id == id);
        return DataSource.Calls.FirstOrDefault(item => item.Id == id); //stage 2
    }

    public Call? Read(Func<Call, bool> filter)
    {
        return DataSource.Calls.FirstOrDefault(filter);
    }

    //public List<Call> ReadAll()
    //{
    //    List<Call> list = new List<Call>();
    //    foreach (var item in DataSource.Calls)
    //        list.Add(item);
    //    return list;
    //}

    //public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) => filter != null
    //? from item in DataSource.Calls
    //  where filter(item)
    //  select item;
    // : from item in DataSource.Calls
    //   select item;
    //}

    //public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2
    //    => filter == null
    //  ? DataSource.Calls.Select(item => item);
    //    : DataSource.Calls.Where(filter);


    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)=> filter == null
? DataSource.Calls.Select(item => item)
: DataSource.Calls.Where(filter);

    public void Update(Call item)
    {
        Call? itemWithId = Read(item.Id);
        if (itemWithId != null)
        {

            DataSource.Calls.Remove(item);
            DataSource.Calls.Add(item);
        }
        else
            throw new Exception($"call with id {item.Id} no exists");



    }
}
