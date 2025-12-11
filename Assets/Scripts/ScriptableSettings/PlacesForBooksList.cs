using System.Collections.Generic;
using UnityEngine;

namespace ScriptableSettings
{
    [CreateAssetMenu(fileName = "PlacesForBooksList", menuName = "Scriptable Objects/PlacesForBooksList")]
    public class PlacesForBooksList : ScriptableObject
    {
        public List<PlacesForBook> placesForBooks;
    }
}