using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController charController;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private float jumpForce;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public Camera camera;
    [SerializeField] private float minLookAngle, maxLookAngle;
    [SerializeField] private LayerMask stockLayer;
    [SerializeField] private LayerMask shelfLayer;
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float throwForce;
    private StockObject heldStockObject;
    private float ySpeed;
    private float horizontalRotation, verticalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // --------------------------------------------------
        //           Get player input for rotation
        // --------------------------------------------------

        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        horizontalRotation += lookInput.x * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f); // Horizontal rotations should only affect the player 

        verticalRotation -= lookInput.y * rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, minLookAngle, maxLookAngle);
        camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f); // Vertical rotations should only affect the camera

        // --------------------------------------------------
        //           Get player input for movement
        // --------------------------------------------------

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>(); // Gets the input made by the player in Vector2 form
        Vector3 verticalMovement = transform.forward * moveInput.y;
        Vector3 horizontalMovement = transform.right * moveInput.x;
        Vector3 moveAmount = (horizontalMovement + verticalMovement).normalized;

        moveAmount *= moveSpeed;

        // --------------------------------------------------
        //   Simulate gravity on the player's y-coordinate
        // --------------------------------------------------

        if(charController.isGrounded == true)
        {
            ySpeed = -2f; // Keeps the player pressed to the ground so isGrounded stays true

            // Check if the player jumped and prevent double jumps
            if(jumpAction.action.WasPressedThisFrame())
            {
                ySpeed = jumpForce;
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime; // Apply gravity if the player isn't grounded
        }
        
        moveAmount.y = ySpeed;
        charController.Move(moveAmount * Time.deltaTime);

        // --------------------------------------------------
        //              Check for stock objects
        // --------------------------------------------------

        // Shoots a ray at half the camera's width and at half the camera's height (center of the screen)
        Ray ray = camera.ViewportPointToRay(new Vector3(.5f, .5f, 0f)); 
        RaycastHit hit;

        if(heldStockObject == null) // Not holding a stock object
        {
            // Pickup the stock object
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                if(Physics.Raycast(ray, out hit, rayDistance, stockLayer))
                {
                    heldStockObject = hit.collider.GetComponent<StockObject>();
                    heldStockObject.transform.SetParent(holdPoint);
                    heldStockObject.Pickup();
                }
            }

            // Pickup a stock object in the shelf
            if(Mouse.current.rightButton.wasPressedThisFrame)
            {
                if(Physics.Raycast(ray, out hit, rayDistance, shelfLayer))
                {
                    heldStockObject = hit.collider.GetComponentInParent<ShelfSpaceController>().GetStock(hit.collider);

                    if(heldStockObject)
                    {
                        heldStockObject.transform.SetParent(holdPoint);
                        heldStockObject.Pickup();
                    }
                }
            }
        }
        else // Holding a stock object 
        {
            // Place the stock object in a shelf
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                if(Physics.Raycast(ray, out hit, rayDistance, shelfLayer))
                {
                    hit.collider.GetComponentInParent<ShelfSpaceController>().PlaceStock(heldStockObject);

                    if(heldStockObject.GetPlaced())
                    {
                        heldStockObject = null;
                    }
                }
            }

            // Drop the stock object
            if(Mouse.current.rightButton.wasPressedThisFrame)
            {
                heldStockObject.Release();
                heldStockObject.GetStockRB().AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
                
                heldStockObject.transform.SetParent(null);
                heldStockObject = null;
            }
        }

        
    }
}
