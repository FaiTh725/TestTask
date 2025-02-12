import styles from "./ClearButton.module.css";

const ClearButton = ({action, children}) => {
  return (
    <div className={styles.ClearButton__Main}>
      <button className={styles.ClearButton__Button}
        onClick={action}>
        {children}
      </button>
    </div>
  )
}

export default ClearButton;