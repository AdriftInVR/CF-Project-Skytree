using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum CombatStatus{
    WAITING_FOR_FIGHTER,
    FIGTHER_ACTION,
    CHECK_FOR_VICTORY,
    NEXT_TURN,
    RUNNING
}


public class CombatManager : MonoBehaviour{
    private Fighter[] fighters;
    private GameObject[] players;
    private int currentFighterIndex;
    private int lastEnemyIndex;
    private int lastPlayerIndex;
    private int index;
    private int turno = 1; // Variable para depuración no hace nada en el estado del juego
    private bool end = false;
    private bool badEnd = false;
    public static bool isCombatActive = false;
    private bool selectorPositioned = true;
    public static bool playerTurn = true;
    public static bool menuOpen = false;
    public static CombatStatus combatStatus;
    public Action currentFigtherAction;
    public static Vector2 direction;
    public int selectedEnemyIndex = 0;  // Índice del enemigo seleccionado
    public int EnemySelect, PlayerSelect;
    public GameObject[] targetArrows; // Prefab de la flecha para indicar el objetivo
    private GameObject currentTargetArrow; // Instancia de la flecha
    private GameObject Wheel;
    private PlayerInput myInput;
    public static InputAction confirmButton;
    public static InputAction cancelButton;
    public List<GameObject> CombatPrefabs ; // Lista de prefabs de enemigos
    public List<Transform> SpawnPoints;   // Lista de puntos de spawn

    void Start(){
        GatherEnemies();
        // Busca a los peleadores en la escena
        fighters = FindObjectsOfType<Fighter>();
        foreach (var fighter in fighters)
        {
            fighter.combatManager = this;
        }
        // Establece el estado del combate
        // Asigna el turno al primer peleador basado en la velocidad
        SortFightersBySpeed(); 
        currentFighterIndex = -1;
        isCombatActive = true;
        myInput = GetComponent<PlayerInput>();
        cancelButton = myInput.actions["Cancel"];
        StartCoroutine(CombatLoop());
    }

    void FixedUpdate()
    {
        if (RunHandle.running)
        {
            combatStatus = CombatStatus.RUNNING;
        }
    }

    void GatherEnemies()
    {
        // Verificar si hay spawn points disponibles
        if (SpawnPoints.Count == 0)
        {
            return;
        }
        // Buscar el prefab correspondiente usando el nombre guardado en CombatData
        GameObject mainEnemyPrefab = CombatPrefabs.Find(prefab => prefab.name == CombatData.EnemyName.Replace("(Clone)", "").Trim());

        if (mainEnemyPrefab != null)
        {
            mainEnemyPrefab.GetComponent<Enemy>().combatManager = GetComponent<CombatManager>();

            // Spawnear al enemigo principal
            SpawnEnemy(mainEnemyPrefab);
        }
        else
        {
            Debug.LogError($"No se encontró un prefab para el enemigo: {CombatData.EnemyName}");
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Seleccionar un spawn point aleatorio
        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)];

        // Instanciar el enemigo en el spawn point seleccionado
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void SortFightersBySpeed(){

        // Ordenar los Personajes por velocidad
        System.Array.Sort(fighters, (fighter1, fighter2) => {

            int speedComparison = fighter2.stats.speed.CompareTo(fighter1.stats.speed);

            // Si las velocidades son iguales, decidir aleatoriamente
            if (speedComparison == 0)
            {
                return Random.Range(0, 2) == 0 ? 1 : -1; // Devuelve 1 o -1 aleatoriamente
            }

            return speedComparison; // Retorna la comparación de velocidad
        });
    }

    IEnumerator CombatLoop(){
        RunHandle.running = false;
        end = false;
        badEnd = false;
        combatStatus = CombatStatus.NEXT_TURN;
        while (isCombatActive){
            switch (combatStatus){
                case CombatStatus.WAITING_FOR_FIGHTER:
                    yield return null;
                    break;
                case CombatStatus.FIGTHER_ACTION:
                    // Ejecutar la habilidad del personaje
                    StartCoroutine(currentFigtherAction.Run());
                    combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                    yield return null;
                    break;

                case CombatStatus.CHECK_FOR_VICTORY:
                    CheckWinLoss();
                    break;

                case CombatStatus.NEXT_TURN:
                    // Buscar al siguiente peleador vivo
                    do {
                        currentFighterIndex = (currentFighterIndex + 1) % fighters.Length;
                    } while (!fighters[currentFighterIndex].isAlive);
                    Fighter currentTurn = fighters[currentFighterIndex];
                    Debug.Log("Turno " + (int)(turno/fighters.Length+1) + " / Es el turno de " + currentTurn.fighterName + ".");
                    turno++; // No hace nada fin de depuración
                    currentTurn.InitTurn();
                    combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                    yield return null;
                    break;
                case CombatStatus.RUNNING:
                    Destroy(Wheel);
                    yield return null;
                    break;
            }   
        }
    }

    void CheckWinLoss()
    {
        int playersAlive = 0;
        int enemiesAlive = 0;
        // Filtrar los personajes del equipo del jugador que aún están vivos y verificar si todos están derrotados
        foreach (var fighter in fighters)
        {
            if (fighter.team == Team.Player && fighter.isAlive)
            {
                playersAlive++;
            }
        }
        foreach (var fighter in fighters)
        {
            if (fighter.team == Team.Enemy && fighter.isAlive)
            {
                enemiesAlive++;
            }
        }
        if (enemiesAlive == 0)
        {
            end = true;
            Debug.Log("El equipo enemigo ha sido derrotado.");
        }
        if (playersAlive == 0)
        {
            badEnd = true;
            Debug.Log("El equipo del jugador ha sido derrotado.");
        }
        if (end)
        {
            PlayerController.locked = true;
            isCombatActive = false;
            StartCoroutine(WaitForEffects());
        }
        if (badEnd)
        {
            isCombatActive = false;
            StartCoroutine(WaitForDeaths());
        }
        else
        {
            combatStatus = CombatStatus.NEXT_TURN;
        }
    }

    IEnumerator WaitForEffects()
    {
        yield return new WaitForSeconds(1f);
        GameObject[] effects;
        do
        {
            effects = GameObject.FindGameObjectsWithTag("VFX");
            yield return new WaitForSeconds(1f);
        } while (effects != null && effects[0] != null );
        Scene activeScene = SceneManager.GetActiveScene();
        yield return null;
        if (activeScene.name.Substring(0,4) == "Boss")
        {
            // If its a  boss scene, load the "SecuenciaFinal" scene
            SceneManager.LoadScene("SecuenciaFinal");
        }
        else
        {
            // If its a combate scene, load the "Exploracion" scene
            SceneManager.LoadScene("Exploracion");
        }
    }

    IEnumerator WaitForDeaths()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Derrota");
    }

    // Método para obtener un peleador opuesto al actual en el turno - IA
    public Fighter GetOppositeFighter(Team team) {
        // Filtrar los personajes del equipo enemigo que aún están vivos
        var opposingTeam = System.Array.FindAll(this.fighters, fighter => fighter.team != team && fighter.isAlive);

        // Si hay personajes del equipo enemigo vivos, selecciona uno aleatoriamente
        if (opposingTeam.Length > 0) {
            return opposingTeam[Random.Range(0, opposingTeam.Length)];
        }
        
        // Si no hay personajes vivos en el equipo enemigo, retorna null
        return null;
    }


    // Método para obtener un peleador aliado al actual en el turno - IA
    public Fighter GetTeamFighter(Team team) {
        // Filtrar los personajes del equipo aliad que aún están vivos
        var allyTeam = System.Array.FindAll(this.fighters, fighter => fighter.team == team && fighter.isAlive);

        // Si hay personajes del equipo aliado vivos, selecciona uno aleatoriamente
        if (allyTeam.Length > 0) {
            return allyTeam[Random.Range(0, allyTeam.Length)];
        }

        // Si no hay personajes vivos en el equipo aliado, retorna null
        return null;
    }

    public void OnFighterAction(Action action){
        currentFigtherAction = action;
        combatStatus = CombatStatus.FIGTHER_ACTION;
    }

    // Método para seleccionar un objetivo enemigo con las teclas de flecha y Enter
    IEnumerator HandleTargetSelection(Player player, Action action, Fighter[] targets) {
        PlayerController.locked = true;
        selectedEnemyIndex = 0;  // Inicia en el primer enemigo
        Vector3 arrowScale = new Vector3(0,0,0);
        selectedEnemyIndex = Mathf.Clamp(0, targets.Length-1, selectedEnemyIndex);
        if (targets[0].team == Team.Player)
        {
            arrowScale = new Vector3(7f,7f,7f);
            selectedEnemyIndex = lastPlayerIndex;
        }
        else
        {
            arrowScale = new Vector3(-7f,7f,7f);
            selectedEnemyIndex = lastEnemyIndex;
        }

        if (targets.Length-1 < selectedEnemyIndex)
        {
            selectedEnemyIndex = 0;
        }

        GameObject currentTargetArrow = Instantiate(targetArrows[index]);
        currentTargetArrow.transform.localScale = arrowScale;

        HighlightTarget(targets[selectedEnemyIndex]);  // Resalta el primer objetivo seleccionado
        SetArrowToTarget(currentTargetArrow, targets[selectedEnemyIndex]);

        // Posicionar la flecha sobre el enemigo seleccionado
        int oldIndex = selectedEnemyIndex; 
        while (!cancelButton.WasPressedThisFrame()) {
            oldIndex = selectedEnemyIndex;
            if (Mathf.Abs(direction.y)>0 && selectorPositioned) {
                selectedEnemyIndex = (selectedEnemyIndex + (int)Mathf.Sign(direction.y) + targets.Length) % targets.Length;  // Mover hacia arriba
                HighlightTarget(targets[selectedEnemyIndex]);
                if (oldIndex != selectedEnemyIndex)
                {
                    StartCoroutine(MoveArrowToTarget(currentTargetArrow, targets[selectedEnemyIndex]));
                }
            }
            if (confirmButton.WasPressedThisFrame()) {  // Seleccionar con Enter
                action.SetEmitterAndReceiver(player, targets[selectedEnemyIndex]);

                // Añadir lógica para activar las animaciones correctas según el tipo de acción
                if (action.isTeamAction) {
                    // Si es una acción de equipo (curación), activar la animación de curación
                    player.anim.SetTrigger("Heal");
                } else {
                    // Si es una acción de ataque, activar la animación de ataque
                    player.anim.SetTrigger("Attack");
                }

                if (targets[selectedEnemyIndex].team == Team.Player)
                {
                    lastPlayerIndex = selectedEnemyIndex;
                }
                else
                {
                    lastEnemyIndex = selectedEnemyIndex;
                }

                OnFighterAction(action);

                // Destruir la flecha al confirmar la selección
                Destroy(currentTargetArrow);
                Destroy(Wheel);
                player.currentAction = null;
                break;
            }
            yield return null;
        }
        if(cancelButton.WasPressedThisFrame())
        {
            Destroy(currentTargetArrow);
            player.currentAction = null;
            StartCoroutine(PlayerTurn(player));
            PanelHandler.ReopenActive();
            combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
        }
    }

    // Actualizar el método para decidir los objetivos
    public IEnumerator PlayerTurn(Player player) {
        WheelSelection.lastCube = player.lastCube;
        confirmButton = myInput.actions[player.fighterName];
        Vector3 offset = player.transform.position + new Vector3(0f,12.5f,7.5f);
        if(!Wheel)
        {
            PlayerController.locked = true;
            yield return new WaitForSeconds(0.5f);
            Wheel = Instantiate(player.ActionWheel, offset, player.ActionWheel.transform.rotation);
        }
        index = player.fighterName == "Timber" ? 0 : 1;
        Fighter[] possibleTargets;
        while (player.currentAction == null)
        {
            yield return null;
        }
        PanelHandler.ClosePanel();
        WheelSelection.lockedRotation = true;
        if (player.currentAction.isTeamAction) {
            // Si es una acción de equipo, permite seleccionar entre aliados
            possibleTargets = System.Array.FindAll(fighters, f => f.team == player.team && f.isAlive);
        } else {
            // Si es una acción normal, seleccionar enemigos
            possibleTargets = System.Array.FindAll(fighters, f => f.team != player.team && f.isAlive);
        }
        StartCoroutine(HandleTargetSelection(player, player.currentAction, possibleTargets));
        yield return null;
    }

    void SetArrowToTarget(GameObject arrow, Fighter target) {
        Vector3 offset = new Vector3(0f,9f,0f);
        if (target.team == Team.Player)
        {
            offset.x = 5f;
        }
        else
        {
            offset.x = -8f;
        }
        arrow.transform.position = target.transform.position + offset;
        selectorPositioned = true;
    }

    IEnumerator MoveArrowToTarget(GameObject arrow, Fighter target) {
        Vector3 offset = new Vector3(0,9f,0);
        if (target.team == Team.Player)
        {
            offset.x += 5f;
        }
        else
        {
            offset.x -= 6f;
        }
        Vector3 targetPos = target.transform.position + offset;
        selectorPositioned = false;
        while (!selectorPositioned && arrow != null){
            arrow.transform.position = Vector3.Lerp(arrow.transform.position, targetPos,0.25f);
            if (arrow.transform.position == targetPos)
            {
                break;
            }
            yield return null;
        }
        selectorPositioned = true;
        yield return null;
    }

    // Método para resaltar el objetivo actual (puedes personalizar el resaltado)
    void HighlightTarget(Fighter target) {
        Debug.Log("Objetivo seleccionado: " + target.fighterName);
        //  Añadir efectos visuales, como cambiar el color del enemigo, para que el jugador vea qué enemigo está seleccionado o agregar un pregab de flecha.
    }

    void OnDirection()
    {
        direction = myInput.actions["Direction"].ReadValue<Vector2>();
    }
}
