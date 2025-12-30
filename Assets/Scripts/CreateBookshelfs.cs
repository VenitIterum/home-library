using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Database;
using Database.Tables;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class CreateBookshelfs : MonoBehaviour
{
    [SerializeField]
    private Transform room;

    [SerializeField]
    private GameObject bookPrefab;

    [SerializeField]
    private bool IsAddNewBook;
    
    [SerializeField]
    private bool IsAddNewAuthor;

    [SerializeField]
    private bool IsAddNewGenre;

    //Добавлять в него полки из room
    private List<GameObject> sectionsList = new List<GameObject>();
    private List<GameObject> placesBookshelfList = new List<GameObject>();

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
        GameObject currentBookshelf = null;
        BookshelfPlace allBookshelfs = null;

        List<GameObject> activeBookshelfList = new List<GameObject>();
        List<Book> booksList = libraryManager.GetBooksList(db);
        int booksCountInDatabase = booksList.Count;
        int countBooksInBookshelf = 0;

        //For TESTS
        AddDatasToTables();

        float addValue = -33.4f;

        while (!IsFindFinalBookshelf)
        {
            GameObject sectionPrefab = room.GetComponent<RoomGenerator>().sectionPrefab;
            sectionsList.Add(GameObject.Instantiate(sectionPrefab, room.GetComponent<RoomGenerator>().roomSection.transform));
            Debug.Log($"<color=orange>Количество секций: {sectionsList.Count}</color>");

            addValue += 33.4f;
            sectionsList[sectionsList.Count - 1].transform.localPosition += new Vector3(addValue, .0f, .0f);
            room.GetComponent<RoomGenerator>().rightWall.transform.localPosition += new Vector3(addValue, .0f, .0f);
            
            foreach (Transform bookshelf in sectionsList[sectionsList.Count - 1].GetComponent<Section>().bookshelfs.transform)
            {
                placesBookshelfList.Add(bookshelf.gameObject);
            }
            Debug.Log($"<color=magenta>Количество полок: {placesBookshelfList.Count}</color>");

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

            //DELETE
            // if (IsFindFinalBookshelf)
            // {
            //     // sectionsList.Add(GameObject.Instantiate(sectionPrefab, room.GetComponent<RoomGenerator>().roomSection.transform));
            //     // Debug.Log($"<color=orange>Количество секций: {sectionsList.Count}</color>");
            //     break;
            // }

            //DELETE
            Debug.Log($"activeBookshelfList: {activeBookshelfList.Count}");
        }

        //ТЕСТЫ.
        //Книги спавнятся в первой секции. Во второй секции не активируются полки
        foreach (var element in activeBookshelfList)
        {
            Debug.Log($"Name: {element.transform.position}");
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
            for (int i = 0; i < 20; i++)
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
