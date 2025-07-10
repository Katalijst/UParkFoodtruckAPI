# UParkFoodtruckAPI

**UParkFoodtruckAPI** est une API REST légère permettant de gérer les réservations de places de stationnement pour foodtrucks. Elle permet de créer, annuler, consulter les réservations actives, et générer des rapports mensuels.

---

## 🚀 Points d'entrée de l'API

### 🔹 Créer une réservation

`POST /booking`

Crée une nouvelle réservation si une place est disponible.

#### Corps de la requête (`CreateBookingRequest`)
```json
{
  "foodtruckId": "ABC-123",
  "size": "Half",
  "date": "2025-07-12"
}
````

* `foodtruckId` (string) : Immatriculation du foodtruck.
* `size` (`ParkingSize` enum) : Taille de la place demandée (`Half`, `Full`).
* `date` (string) : Date souhaitée au format `yyyy-MM-dd`.

#### Réponses

* `201 Created` : Réservation créée.
* `400 Bad Request` : Erreur de validation ou aucune place disponible.

---

### 🔹 Annuler une réservation

`POST /booking/cancel/{id}`

Annule une réservation existante par ID. Si l’annulation est effectuée plus de 2 jours à l’avance, un remboursement est appliqué.

#### Paramètre d’URL

* `id` (int) : Identifiant de la réservation.

#### Réponses

* `200 OK` : Réservation annulée avec succès.
* `400 Bad Request` : Réservation introuvable ou déjà annulée.

---

### 🔹 Récupérer les réservations actives

`GET /booking/active`

Retourne toutes les réservations valides (non annulées).

#### Exemple de réponse

```json
[
  {
    "id": 5,
    "parkingSlot": 3,
    "foodtruckId": "XYZ-789",
    "size": "Half",
    "status": "Valid",
    "date": "2025-07-10",
    "cost": 20.0,
    "isPaid": false
  }
]
```

* `200 OK` : Liste des réservations actives.
* `400 Bad Request` : Erreur lors de la récupération.

---

### 🔹 Générer un rapport mensuel

`GET /booking/report/{year}/{month}`

Génère un rapport d’utilisation mensuel par foodtruck.

#### Paramètres d’URL

* `year` (int) : Année (ex. `2025`)
* `month` (int) : Mois (ex. `7` pour juillet)

#### Exemple de réponse

```json
[
  {
    "foodtruckId": "ABC-123",
    "entries": [
      {
        "date": "2025-07-03",
        "slot": 2,
        "status": "Valid",
        "cost": 20.0,
        "isPaid": true
      }
    ],
    "totalCost": 20.0,
    "totalPaid": 20.0
  }
]
```

* `200 OK` : Rapport généré.
* `400 Bad Request` : Erreur de génération.

---

## 🧠 Règles métier

* La réservation doit être effectuée **dans un délai de 7 jours** à partir d’aujourd’hui.
* **Les vendredis sont bloqués** pour réservation.
* Il y a **7 emplacements maximum** par jour, chacun avec une capacité de `Full`.
* Si l’annulation a lieu **au moins 2 jours à l’avance**, le montant est **remboursé**.
* Le coût est :

  * `20 €` pour une réservation anticipée.
  * `40 €` pour une réservation le jour même.

---

## 📦 Modèles de données

### `BookingResponse`

```json
{
  "id": 1,
  "parkingSlot": 4,
  "foodtruckId": "ABC-123",
  "size": "Half",
  "status": "Valid",
  "date": "2025-07-12",
  "cost": 20.0,
  "isPaid": false
}
```

### `MonthlyReportResponse`

```json
{
  "foodtruckId": "ABC-123",
  "entries": [ { /* voir MonthlyReportEntry */ } ],
  "totalCost": 100.0,
  "totalPaid": 80.0
}
```

---

## 📘 Énumérations

### `ParkingSize`

* `Half` = 1
* `Full` = 2

### `BookingStatus`

* `Valid` : Réservation valide.
* `Cancelled` : Réservation annulée.

---

## 📄 Documentation Swagger

L’API est documentée avec Swagger (OpenAPI). Vous pouvez accéder à l’interface via :

```
https://localhost:{port}/swagger
```

---
