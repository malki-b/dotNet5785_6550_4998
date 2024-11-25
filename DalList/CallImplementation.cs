using System;

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class CallImplementation : ICall
{
    public void Create(Call item)
    {
       int newId = Config.NextCallId;
       Call newCall= item with { Id = newId };
       DataSource.Calls.Add(newCall);
    }
    
    public void Delete(int id)
    {
       Call? currentCall=Read(id);
        if (currentCall != null)
         DataSource.Calls.Remove(currentCall);
        else
            throw new Exception($"volunteer with id {id} no exists");
        
    }

    public void DeleteAll()
    {
       DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
       return DataSource.Calls.Find(c=>c.Id == id);
    }

    public List<Call> ReadAll()
    {
        List<Call> list = new List<Call>();
        foreach (var item in DataSource.Calls)
            list.Add(item);
       return list;
    }

    public void Update(Call item)
    {
        Call? itemWithId = Read(item.Id);
        if (itemWithId != null)
        {
       
            DataSource.Calls.Remove(item);
            DataSource.Calls.Add(item);
        }
        else
          throw new Exception($"volunteer with id {item.Id} no exists");

     

    }
}
