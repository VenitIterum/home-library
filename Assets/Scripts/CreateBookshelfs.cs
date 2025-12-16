using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class CreateBookshelfs : MonoBehaviour
{
    [SerializeField]
    private Transform room;

    [SerializeField]
    private GameObject bookPrefab;

    [SerializeField]
    private List<GameObject> placesForBooksList;

    [SerializeField]
    private bool IsAddNewBook;

    private SQLite.SQLiteConnection db;
    LibraryManager libraryManager = new LibraryManager();


    private void Awake()
    {
        db = libraryManager.Init();

        if (IsAddNewBook)
        {
            libraryManager.AddBook(db, "Хоббит, или Туда и обратно", "Джон Р. Р. Толкин", 1937, true, 5);
        }
    }

    private void Start()
    {
        libraryManager.CreateTables(db);
        libraryManager.TestDatabase(db);

        List<Book> booksList = libraryManager.GetBooksTitles(db);
        // database.RequestToDatabase("");

        SpawnPoints placesForBooksGetSpawn = null;
        int countBooksInPlace = 0;

        foreach (var placeOnScene in placesForBooksList)
        {
            placesForBooksGetSpawn = placeOnScene.GetComponent<SpawnPoints>();

            for (int i = 0; i < placesForBooksGetSpawn.SpawnPointsList.Count; i++)
            {
                countBooksInPlace += placesForBooksGetSpawn.SpawnPointsList[i].booksCount;
            }

            //TODO Надо бы ловить ошибку, если лимит будет привышен. Что в таком случае делать?
            //Больше мест для книг или ставить ограничение.
            if (booksList.Count <= countBooksInPlace)
            {
                placeOnScene.SetActive(true);
                break;
            }
            
            countBooksInPlace = 0;
        }

        int countBooksInPoint = 0;
        int booksCount = 0;
        float shiftPosition = .0f;

        foreach (var point in placesForBooksGetSpawn.SpawnPointsList)
        {
            countBooksInPoint = point.booksCount;
            shiftPosition = .0f;

            while (countBooksInPoint > 0 && booksCount < booksList.Count)
            {
                shiftPosition += .3f;
                BookGenerator(point.point, booksList[booksCount].Id + ". " + booksList[booksCount].Title, booksList[booksCount].Author, Random.Range(5, 1000), Random.Range(5, 30), shiftPosition);
                countBooksInPoint--;
                booksCount++;
            }
        }
    }

    private void BookGenerator(GameObject currentBookshelf, string bookName = "Book Name", string bookAuthor = "Book Author", float pageCount = 100, float hight = 25, float shiftPosition = .0f)
    {
        float bookWidth = 0;
        float bookHeight = 0;
        
        if (pageCount/1000 < 0.2) bookWidth = 0.2f;
        else if (pageCount/1000 > 0.3) bookWidth = 0.3f;
        else bookWidth = pageCount/1000;

        if (hight/30 < 0.5) bookHeight = 0.5f;
        else if (hight/30 > 0.9) bookHeight = 0.9f;
        else bookHeight = hight/30;

        GameObject book = GameObject.Instantiate(bookPrefab, currentBookshelf.transform);
        book.GetComponent<BoxCollider2D>().size = new Vector2(bookWidth, bookHeight);
        book.transform.GetChild(0).localScale = new Vector2(bookWidth, bookHeight);
        book.transform.localPosition = new Vector2(book.transform.localPosition.x + shiftPosition, book.transform.localPosition.y);
        book.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(bookHeight, bookWidth);
        book.transform.GetChild(1).GetComponent<TextMeshPro>().text = bookName;
        // book.transform.localRotation = Quaternion.Euler(0, 0, 45f);
    }
}
