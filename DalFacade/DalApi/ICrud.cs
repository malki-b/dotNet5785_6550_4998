using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Defining a generic common interface that will define all CRUD operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrud<T> where T : class
{
    void Create(T item); //Creates new entity object in DAL
    T? Read(int id); //Reads entity object by its ID 
                     // List<T> ReadAll(); //stage 1 only, Reads all entity objects
    /// <summary>
    /// Update the existing ReadAll method of the CRUD interface so that it
    /// returns a filtered list of objects, based on a parameter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); // stage 2

    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
    void DeleteAll(); //Delete all entity objects

    /// <summary>
    /// Added a method to the CRUD interface to return an object, not only by Id,
    /// but also by another filter parameter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    T? Read(Func<T, bool> filter); // stage 2
}
