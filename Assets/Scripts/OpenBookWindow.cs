using UnityEngine;
using Database;
using Database.Tables;
using System.Collections.Generic;
using TMPro;

public class OpenBookWindow : MonoBehaviour
{
    private SQLite.SQLiteConnection db;
    private LibraryManager libraryManager = new LibraryManager();

    public void OnBookClicked()
    {
        string currentBookTitle = this.GetComponentInChildren<TextMeshPro>().text;

        Book currentBook = libraryManager.FindBookByExactTitle(db, currentBookTitle);

        if(currentBook == null) Debug.Log($"<color=red>Такой книги нету - {currentBookTitle}!</color>");
        else
        {
            Author currentAuthor = libraryManager.FindAuthorById(db, currentBook.Author);
            Debug.Log($"Ты нажал на КНИГУ: {currentBook.Title}; Автор: {currentAuthor.LastName} {currentAuthor.FirstName}; Год написания: {currentBook.YearWriting}");
        }
    }
}