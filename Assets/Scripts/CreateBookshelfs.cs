using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Database;
using Database.Tables;

public class CreateBookshelfs : MonoBehaviour
{
    [SerializeField]
    private Transform room;

    [SerializeField]
    private GameObject bookPrefab;

    //Будем добывать эту переменную из room (см. выше), а этот список убрать из едитора
    //и добавлять в него полки из room
    [SerializeField]
    private List<GameObject> placesBookshelfList;

    [SerializeField]
    private bool IsAddNewBook;
    
    [SerializeField]
    private bool IsAddNewAuthor;

    [SerializeField]
    private bool IsAddNewGenre;

    private SQLite.SQLiteConnection db;
    private LibraryManager libraryManager = new LibraryManager();

    private float shiftPosition = .0f;

    private void Awake()
    {
        if (db == null) db = libraryManager.Init();
    }

    private void Start()
    {
        bool IsFindFinalBookshelf = false; 
        int countBooksInBookshelf = 0;
        GameObject currentBookshelf = null;
        List<GameObject> activeBookshelfList = new List<GameObject>();

        //For TESTS
        AddDatasToTables();

        List<Book> booksList = libraryManager.GetBooksList(db);
        int booksCountInDatabase = booksList.Count;

        BookshelfPlace allBookshelfs = null;

        foreach (var bookshelfs in placesBookshelfList)
        {
            allBookshelfs = bookshelfs.GetComponent<BookshelfPlace>();
            SpawnPoints allSpawnPoints = null;

            foreach (var bookshelf in allBookshelfs.bookshelfList)
            {
                currentBookshelf = bookshelf;
                countBooksInBookshelf = 0; //в дальнейшем тут должна быть длина
                allSpawnPoints = currentBookshelf.GetComponent<SpawnPoints>();

                foreach (var point in allSpawnPoints.SpawnPointsList)
                {
                    countBooksInBookshelf += point.booksCount;
                }

                if (booksCountInDatabase <= countBooksInBookshelf)
                {
                    currentBookshelf.SetActive(true);
                    activeBookshelfList.Add(currentBookshelf);
                    IsFindFinalBookshelf = true;
                    break;
                }
            }
            
            if (IsFindFinalBookshelf) break;

            currentBookshelf.SetActive(true);
            activeBookshelfList.Add(currentBookshelf);
            booksCountInDatabase -= countBooksInBookshelf;
        }

        if (!IsFindFinalBookshelf)
        {
            Debug.LogWarning("Не для всех книг хватило места!");
            return;
        }

        int countBooksInPoint = 0;
        int booksCount = 0;

        foreach (var bookshelf in activeBookshelfList)
        {
            foreach (var point in bookshelf.GetComponent<SpawnPoints>().SpawnPointsList)
            {
                shiftPosition = .0f;
                countBooksInPoint = point.booksCount;

                while (countBooksInPoint > 0 && booksCount < booksList.Count)
                {
                    BookGenerator(point.point, booksList[booksCount].Id + ". " + booksList[booksCount].Title, booksList[booksCount].PageCount, booksList[booksCount].BookHight);
                    countBooksInPoint--;
                    booksCount++;
                }
            }
        }
    }

    private void BookGenerator(GameObject currentBookshelf, string bookName = "Book Name", float pageCount = 500, float hight = 20)
    {
        float bookWidth = 0;
        float bookHeight = 0;
        
        // if (pageCount/1000 < 0.2) bookWidth = 0.2f;
        // else if (pageCount/1000 > 0.3) bookWidth = 0.3f;
        // else bookWidth = pageCount/1000;

        // if (hight/30 < 0.5) bookHeight = 0.5f;
        // else if (hight/30 > 0.9) bookHeight = 0.9f;
        // else bookHeight = hight/30;

        bookWidth = pageCount/5000;
        bookHeight = hight/25;
        shiftPosition += bookWidth/2;

        GameObject book = GameObject.Instantiate(bookPrefab, currentBookshelf.transform);
        book.GetComponent<BoxCollider2D>().size = new Vector2(bookWidth, bookHeight);
        book.transform.GetChild(0).localScale = new Vector2(bookWidth, bookHeight);
        book.transform.localPosition = new Vector2(book.transform.localPosition.x + shiftPosition, book.transform.localPosition.y);
        book.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(bookHeight - ((20.0f / 100.0f) * bookHeight), bookWidth - ((20.0f / 100.0f) * bookWidth));
        book.transform.GetChild(1).GetComponent<TextMeshPro>().text = bookName;
        // book.transform.localRotation = Quaternion.Euler(0, 0, 45f);
        shiftPosition += bookWidth/2;
    }

    private void AddDatasToTables()
    {
        if (IsAddNewBook)
        {
            for (int i = 0; i < 10; i++)
            {
                libraryManager.AddBook(db, "Тест", 3, Random.Range(1930, 2025), false, Random.Range(0, 5), Random.Range(400, 800), Random.Range(15, 23));
            }
            // libraryManager.AddBook(db, "Обитель апельсинового дерева", 2, 2019, true, 5, 800, 20);
        }

        if (IsAddNewAuthor)
        {
            libraryManager.AddAuthor(db, "Тест", "Тестович");
        }

        if (IsAddNewGenre)
        {
            libraryManager.AddGenre(db, "Фэнтези");
        }
    }
}
