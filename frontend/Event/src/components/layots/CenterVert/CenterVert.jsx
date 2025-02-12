import styles from "./CenterVert.module.css";

const CenterVert = ({children}) => {
  return (
    <div className={styles.CenterVert__Main}>
      <div className={styles.CenterVert__Wrapper}>
        {children}
      </div>
    </div>
  )
}

export default CenterVert;