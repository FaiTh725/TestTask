import Header from "../../Header/Header";
import CenterVert from "../CenterVert/CenterVert";
import styles from "./MainLayout.module.css"

const MainLayout = ({children}) => {
  return (
    <div className={styles.MainLayout__Main}>
      <Header/>
      <CenterVert>
        {children}
      </CenterVert>
    </div>
  )
}

export default MainLayout;