using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ScriptableSettings;
using System.Collections.Generic;

public class CreateBookshelfs : MonoBehaviour
{
    [SerializeField]
    private int bookCount = 26;

    [SerializeField]
    private Transform room;

    [SerializeField]
    private GameObject bookPrefab;

    [SerializeField]
    private List<GameObject> placesForBooksList;


    private void Start()
    {
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
            if (bookCount <= countBooksInPlace)
            {
                placeOnScene.SetActive(true);
                break;
            }
            
            countBooksInPlace = 0;
        }

        int countBooksInPoint = 0;
        float shiftPosition = .0f;

        foreach (var point in placesForBooksGetSpawn.SpawnPointsList)
        {
            countBooksInPoint = point.booksCount;
            shiftPosition = .0f;

            while (countBooksInPoint > 0 && bookCount > 0)
            {
                shiftPosition += .3f;
                BookGenerator(point.point, bookCount + " Book name", "Gerbert Uals", Random.Range(5, 1000), Random.Range(5, 30), shiftPosition);
                countBooksInPoint--;
                bookCount--;
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
