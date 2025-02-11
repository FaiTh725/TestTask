import styles from "./CenterScreen.module.css";

const CenterScreen = ({children}) => {
  return (
    <div className={styles.CenterScreen__Main}>
      <div className={styles.CenterScreen__Body}>
        {children}
      </div>
    </div>
  )
}

export default CenterScreen;