using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CreateBookshelfs : MonoBehaviour
{
[SerializeField]
    private int bookCount = 26;

    [SerializeField]
    private Transform bookcaseParent;

    [SerializeField]
    private GameObject bookshelfPrefab;

    [SerializeField]
    private GameObject bookPrefab;

    private void Start()
    {
        float coordinateNewBookshelf = 0.0f;
        int bookshelfsCount = ((bookCount - (bookCount % 10)) / 10) +1;
        int count = 0;
        int randomCountBooksOnBookShelfs = 0;

        while (bookCount > 0)
        {
            GameObject currentBookshelf = GameObject.Instantiate(bookshelfPrefab, bookcaseParent);
            currentBookshelf.transform.localPosition = new Vector2(0.0f, coordinateNewBookshelf);
            coordinateNewBookshelf -= 4.0f;

            randomCountBooksOnBookShelfs = Random.Range(3, 10);

            while (bookCount > 0)
            {
                if (count == randomCountBooksOnBookShelfs) break;

                BookGenerator(currentBookshelf, "The invisiable man", "Gerbert Uals", Random.Range(5, 1000), Random.Range(5, 30));
                count++;
                bookCount--;
            }
            count = 0;
        }
    }

    private void BookGenerator(GameObject currentBookshelf, string bookName = "Book Name", string bookAuthor = "Book Author", float pageCount = 100, float hight = 25)
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
        book.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(bookHeight, bookWidth);
        book.transform.GetChild(1).GetComponent<TextMeshPro>().text = bookName;
        book.transform.localRotation = Quaternion.Euler(0, 0, 45f);
    }
}
