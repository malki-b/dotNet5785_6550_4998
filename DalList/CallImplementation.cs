using System;

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class CallImplementation : ICall
{

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Call item)
    {
        int newId = Config.NextCallId;
        Call newCall = item with { Id = newId };
        DataSource.Calls.Add(newCall);
        //Console.WriteLine($"your id is {newId}");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        Call? currentCall = Read(id);
        if (currentCall != null)
            DataSource.Calls.Remove(currentCall);
        else
            throw new DalDoesNotExistException($"Call with id {id} no exists");
        //האם לשים DalDeletionImpossible או  DalDoesNotExistException

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Call? Read(int id)
    {
        //return DataSource.Calls.Find(c => c.Id == id);
        return DataSource.Calls.FirstOrDefault(item => item.Id == id); //stage 2
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)=> filter == null
? DataSource.Calls.Select(item => item)
: DataSource.Calls.Where(filter);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Call item)
    {
        Call? itemWithId = Read(item.Id);
        if (itemWithId != null)
        {

            DataSource.Calls.Remove(item);
            DataSource.Calls.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Call with id {item.Id} no exists");
    }
}
