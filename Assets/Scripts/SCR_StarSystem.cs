using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 public class SCR_StarSystem : MonoBehaviour
 {
     [Header("Estrellas")]
     public Image[] estrellas = new Image[3];
     [Header("Sprites de las estrellas")]
     public Sprite estrellaFull;
     public Sprite estrellaHalf;
     public Sprite estrellaNone;

     public void FillStarsWithScore(int _score)
     {
         switch (_score)
         {
             case 6:
                 {
                     foreach (Image img in estrellas)
                         img.sprite = estrellaFull;
                 }
                 break;
             case 5:
                 {
                     estrellas[0].sprite = estrellas[1].sprite = estrellaFull;
                     estrellas[2].sprite = estrellaHalf;
                 }
                 break;
             case 4:
                 {
                     estrellas[0].sprite = estrellas[1].sprite = estrellaFull;
                     estrellas[2].sprite = estrellaNone;
                 }
                 break;
             case 3:
                 {
                     estrellas[0].sprite = estrellaFull;
                     estrellas[1].sprite = estrellaHalf;
                     estrellas[2].sprite = estrellaNone;
                 }
                 break;
             case 2:
                 {
                     estrellas[0].sprite = estrellaFull;
                     estrellas[1].sprite = estrellas[2].sprite = estrellaNone;

                 }
                 break;
             case 1:
                 {
                     estrellas[0].sprite = estrellaHalf;
                     estrellas[1].sprite = estrellas[2].sprite = estrellaNone;
                 }
                 break;
             default:
                 {
                     foreach (Image img in estrellas)
                         img.sprite = estrellaNone;
                 }
                 break;
         }
     }


 }

